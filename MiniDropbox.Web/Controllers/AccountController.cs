using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Data;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Premium;

namespace MiniDropbox.Web.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;
        private EmailHandler _emailHandler;
        
        public AccountController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _emailHandler = new EmailHandler();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View(new AccountLoginModel());
        }
        [HttpPost]
        public ActionResult LogIn(AccountLoginModel model)
        {   
            //var account = Mapper.Map<AccountLoginModel, Account>(model);
            var cuenta = _readOnlyRepository.First<Account>(( x=> x.Email==model.Username && x.Password == model.Password ) );
            if (cuenta != null)
            {
               
                var roles = new List<string>();
                roles.Add("User");
                //cuenta.Roles.Select(x => x.Name).ToList();
                FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                SetAuthenticationCookie(model.Username, roles);
                if (cuenta.Banned == true)
                {
                    var razon = _readOnlyRepository.First<Ban>(x => x.Email == model.Username);
                    Error("La cuenta: "+razon.Email+" ha sido baneada por la siguiente razon: "+razon.Reason);
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
        public ActionResult RecoverAccount()
        {
            return View(new AccountRecoveryModel());
        }
        [HttpPost]
        public ActionResult RecoverAccount(AccountRecoveryModel model)
        {
            var account = _readOnlyRepository.First<Account>(x => x.Email == model.Email);
            if (account != null)
            {
                Success("Se ha enviado un email a ese correo.");
                var token = new Token();
                token.Name = CreateToken(model.Email);
                token.UserId = account.Id;
                DateTime currentDate = DateTime.Now;
                DateTime expdate = currentDate.AddDays(5);
                token.ExpirationDate = expdate;
                _writeOnlyRepository.Create(token);
                _emailHandler.SendEmail(model.Email, "Recupera tu clave", " Se nos ha dicho que perdistes tu contraseña recuperala en http://localhost:1843/account/UpdatePassword?token="+token.Name+" .");
                //Enviar correo a usuario
                return RedirectToAction("LogIn", "Account");
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
                var directory = new Directories();
                directory.Name = account.Email;
                directory.ModifiedDate = DateTime.Now.ToString();
                directory.FileType = "Folder";

                _writeOnlyRepository.Create(directory);
                var dire = _readOnlyRepository.First<Directories>(x => x.Name == account.Email);
                account.RootId = dire.Id;
                dire.Name = "root";
                _writeOnlyRepository.Update(dire);
                account.Admin = false;
                account.Space = 5;
                account.Banned = false;
                _writeOnlyRepository.Create(account);
                _emailHandler.SendEmail(model.Email, "Bienvenido a MiniDropbox","Bienvenido a minidropbox, te has registrado bien.");
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
            _emailHandler.SendEmail(model.Email, " Invitacion a MiniDropbox ", "Edwin Zelaya te ha invitado a formar parte de MiniDropbox, unete Http://localhost:1843/Account/registerAccount?edwin_zelaya5@hotmail.com ");

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
        public ActionResult RecoverPassword(string token)
        {
            var tok = _readOnlyRepository.First<Token>(x => x.Name == token);
            var currentDate = DateTime.Now;
            if (tok == null) return RedirectToAction("LogIn", "Account");
            

            if (tok.ExpirationDate < currentDate)
            {
                Error("Ese token ya expiro, vuelve a pedir recuperar la cuenta");
            }
            else
            {

                var model = new RecoverPasswordModel();
                var account = _readOnlyRepository.First<Account>(x => x.Id == tok.UserId);
                model.Email = account.Email;
                return View(model);
            }
            
            return RedirectToAction("LogIn", "Account");
        }
        [HttpPost]
        public ActionResult RecoverPassword(RecoverPasswordModel model)
        {
           
            if (model.Confirmpassword != model.Password)
            {
                Error("Las contraseñas deben coincidir.");
                return View(model);
            }
            else
            {
                var account = _readOnlyRepository.First<Account>(x => x.Email == model.Email);
                account.Password = model.Password;
                _writeOnlyRepository.Update(account);
                Success("Se ha cambiado su clave con exito.");
            }
            return RedirectToAction("LogIn","Account");
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
       

        public string CreateToken(string email)
        {
            int token = GetHashCode();
            
            string realToken = email + token;
            return realToken;

        }
    }
    
}