using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.WebPages;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Premium;

namespace MiniDropbox.Web.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;
        
        public AccountController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            var account = _readOnlyRepository.GetById<Account>(1);
            
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public ActionResult LogIn(AccountLoginModel model)
        {

            //Mapper.CreateMap<AccountRegisterModel, Account>()
              //  .ForMember(x => x.Email, o => o.MapFrom(y => y.Email));
           
            //var account = Mapper.Map<AccountLoginModel, Account>(model);
            var cuenta = _readOnlyRepository.First<Account>(( x=> x.Email==model.Username && x.Password == model.Password ) );
            if (cuenta != null)
            {
                if (cuenta.Banned == true)
                {
                    Error("Esa cuenta ha sido baneado por violar los terminos de uso");
                    return RedirectToAction("LogIn", "Account");
                }
                if (cuenta.Admin == true)
                {
                    return RedirectToAction("ListUsers", "Administrator");
                }
                return RedirectToAction("ListAllContent", "Disk");
            }
            Error("Lo sentimos, esa cuenta no existe en nuestra base de datos.");
            return View(new AccountLoginModel());
        }

        [HttpGet]
        public ActionResult recoverAccount()
        {
            return View(new AccountRecoveryModel());
        }
        [HttpPost]
        public ActionResult recoverAccount(AccountRecoveryModel model)
        {
            var account = _readOnlyRepository.First<Account>(x => x.Email == model.Email);
            if (account != null)
            {
                Success("Se ha enviado un email a ese correo.");
                SendEmail(model.Email, "Recupera tu clave", " Se nos ha dicho que perdistes tu contraseña recuperala en http://localhost:1843/account/UpdatePassword?"+model.Email+".");
                //Enviar correo a usuario
                return RedirectToAction("ListAllContent", "Disk");
            }
            Error("Lo sentimos, E-Mail no coincide con ninguna de nuestras cuentas.");
            return View(new AccountRecoveryModel());
        }

        [HttpGet]
        public ActionResult registerAccount()
        {
            
            return View(new AccountRegisterModel());
        }
        [HttpPost]
        public ActionResult registerAccount(AccountRegisterModel model)
        {
           //Mapper.CreateMap<AccountRegisterModel, Account>().ForMember(x => x.FirstName, o => o.MapFrom(y => y.FirstName));
            if (checkCuenta(model.Email))
            {
                Error("El correo '" + model.Email + "' ya existe.");
                
                return RedirectToAction("LogIn", "Account"); 
            }
            else if (!checkPassword(model.Password, model.ConfirmPassword))
            {
                Error("Contrasenas no validas: \n" +
                      "1. Las Contraseñas deben coincidir \n" +
                      "2. Las Contraseñas deben ser alfanumericas \n" +
                      "3. La Contraseña debe ser minimo 6 digitos"
                      );
                return View(new AccountRegisterModel());  
            }
            else
            {
                var account = Mapper.Map<AccountRegisterModel, Account>(model);

                account.Admin = false;
                account.Space = 5;
                account.Banned = false;
                _writeOnlyRepository.Create(account);
                SendEmail(model.Email, "Bienvenido a MiniDropbox","Bienvenido a minidropbox, te has registrado bien.");
                Success("El usuario " + model.FirstName + " ha sido registrado.");
                return RedirectToAction("LogIn", "Account");
            }
            
            //RedirectToAction("RegisterAccount", "Account");
        }

        [HttpGet]
        public ActionResult Referral()
        {
            return View(new AccountReferralModel());
        }
        [HttpPost]
        public ActionResult Referral(AccountReferralModel model)
        {
            
            if (checkCuenta(model.Email))
            {
                Error("Ese email ya esta asociado con una cuenta en nuestra base de datos");
                return View(new AccountReferralModel());
            }
            var account = _readOnlyRepository.First<Account>(x => x.Email == "edwin_zelaya5@hotmail.com");
            account.Referidos.Add(model.Email);
            _writeOnlyRepository.Update(account);
            SendEmail(model.Email, " Invitacion a MiniDropbox ", "Edwin Zelaya te ha invitado a formar parte de MiniDropbox, unete Http://localhost:1843/Account/registerAccount?edwin_zelaya5@hotmail.com ");

            Success("Se ha enviado una invitacion a: "+model.Email);
            return View(new AccountReferralModel());
        }

        [HttpGet]
        public ActionResult UpdateProfile()
        {
            //var cuenta = _readOnlyRepository.First<Account>(x => x.Email== )
            //UpdateProfileModel model = new UpdateProfileModel();
            //var account = Mapper.Map<UpdateProfileModel, Account>(cuenta,model);
            //model.Email = account.email;
            //model.FirstName = account.firstname;
            //model.Lastname = account.lastname;
            
            return View(new UpdateProfileModel());
        }

        [HttpPost]
        public ActionResult UpdateProfile(UpdateProfileModel model)
        {
            var account = _readOnlyRepository.First<Account>(x => x.Email == "edwin_zelaya5@hotmail.com");
            if (model.FirstName.IsEmpty() && !model.Lastname.IsEmpty())
            {
                account.LastName = model.Lastname;
            }
            else if (model.Lastname.IsEmpty() && !model.FirstName.IsEmpty())
            {
                account.FirstName = model.FirstName;
            }
            else
            {
                account.FirstName = model.FirstName;
                account.LastName = model.Lastname;
            }
           
            _writeOnlyRepository.Update(account);
            return View( new UpdateProfileModel());
        }

        [HttpGet]
        public ActionResult UpdatePassword()
        {
            return View(new UpdatePasswordModel());
        }
        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordModel model)
        {
            var account = _readOnlyRepository.First<Account>(x => x.Email =="edwin_zelaya5@hotmail.com");
            
            if (!checkPassword(model.NewPassword, model.ConfirmPassword))
            {
                Error("Clave incorrecta.");
                return (UpdatePassword());
            }
            account.Password = model.NewPassword;
            
            _writeOnlyRepository.Update(account);
            Success("Se ha cambiado la clave de usuario "+ account.FirstName);
            return RedirectToAction("UpdateProfile", "Account");
        }

        
        // extra functions
        private bool checkPassword(string password, string confirmPassword)
        {
            Regex r = new Regex("^[a-zA-Z0-9]*$");

            if (r.IsMatch(password))
            {
                if (password.Length >= 6)
                {
                    if (password.Equals(confirmPassword))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool checkCuenta(string email)
        {
            var cuenta = _readOnlyRepository.First<Account>(x => x.Email == email);
            if (cuenta != null)
            {
                return true;
            }
            return false;
        }
        public void SendEmail(string address, string subject, string message)
        {
            
            string email = "postmaster@app5907.mailgun.org";
            string password = "3ipcsv86ayd9";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.mailgun.org", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }
    }
}