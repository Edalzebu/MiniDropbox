using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Xml.Linq;
using BootstrapMvcSample.Controllers;
using FluentNHibernate.Conventions;
using MiniDropbox.Web.Models;
using AutoMapper;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models.Admin;
using FizzWare.NBuilder;
using System.Linq;
using MiniDropbox.Web.Models.Premium;
using NHibernate.Mapping;


namespace MiniDropbox.Web.Controllers
{
    public class AdministratorController : BootstrapBaseController
    {
        //
        // GET: /Administrator/
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public AdministratorController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
        [HttpGet]
        public ActionResult SpaceManager()
        {
            return View(new SpaceManagerModel());
        }
        [HttpPost]
        public ActionResult SpaceManager(SpaceManagerModel model)
        {
            
            var account = _readOnlyRepository.First<Account>(x => x.Email == model.Nombre);
            if (account == null)
            {
                Error("Esa cuenta no existe");
                return View(new SpaceManagerModel());
            }
            else
            {
                if (account.SpaceUsed > model.Space)
                {
                    Error("El usuario ya ha usado: "+ account.SpaceUsed +" , no le puedes poner menos de lo que ya ha usado");
                    return View(new SpaceManagerModel());
                }
            }

            account.Space = model.Space;
            _writeOnlyRepository.Update(account);
            SendEmail(model.Nombre, "Modificacion de Espacio en Minidropbox", "Se le ha hecho un cambio a su capacidad de almacenamiento en MiniDropbox, " +
                                                                              " Su espacio en total ahora es de:  "+model.Space+ " Si piensa que este cambio es injusto pues no nos importa porque con esto termino mi primera entrega :)))))))");
            Success("El espacio de el usuario: " + model.Nombre + " ha sido cambiado a: " + model.Space);
            return RedirectToAction("ListUsers", "Administrator");
        }


        [HttpGet]
        public ActionResult BanUser()
        {
            return View(new BanUserModel());
        }
        [HttpPost]
        public ActionResult BanUser(BanUserModel model)
        {
            var account = _readOnlyRepository.First<Account>(x => x.Email == model.Usuario);
            account.Banned = true;
            _writeOnlyRepository.Update(account);

            var ban = Mapper.Map<BanUserModel, Ban>(model);
            ban.Reason = model.Razon;
            ban.Email = model.Usuario;
            ban.Administrator = "edwin_zelaya5@hotmail.com"; 
            _writeOnlyRepository.Create(ban);
            SendEmail(model.Usuario,"Usted ha sido baneado de Minidropbox", " Esta cuenta esta baneada de Minidropbox, por no seguir los terminos de MiniDropbox, su cuenta fue baneada por: "+ban.Administrator+" por la siguiente razon: "+ban.Reason);
            Success("El usuario:" +model.Usuario+ " ha sido baneado. ");

            return RedirectToAction("ListUsers","Administrator");
        }


        [HttpGet]
        public ActionResult ListUsers()
        {
            List<Account> listaUsuarios = _readOnlyRepository.GetAllAccounts<Account>().ToList();
            //List<Account> listaUsuarios = _readOnlyRepository.Query<Account>(x => x.Email == "*").ToList();
            List<ListUsersModel> lista = new List<ListUsersModel>();

            for (int i = 0; i < listaUsuarios.Count; i++)
            {
                
                ListUsersModel model = new ListUsersModel();
                var user = listaUsuarios.ElementAt(i);
                model.Name = user.FirstName;
                model.LastName = user.LastName;
                model.email = user.Email;
                model.availableSpace = user.Space;
                lista.Add(model);
            }
            //var listOfContent = Builder<ListUsersModel>.CreateListOfSize(10).Build().ToList();
            return View(lista);
        }
        [HttpPost]
        public ActionResult ListUsers(ListUsersModel model)
        {
            return View(new ListUsersModel());
        }

        [HttpGet]
        public ActionResult PremiumPackage()
        {
            return View(new PremiumPackageModel());
        }

        [HttpPost]
        public ActionResult PremiumPackage(PremiumPackageModel model)
        {
            if (model.Name.Length >= 6)
            {
                if (model.Description.Length >= 25)
                {
                    var package = Mapper.Map<PremiumPackageModel, PremiumPackage>(model);
                    package.Available = true;
                    _writeOnlyRepository.Create(package);
                    Success("El paquete: "+model.Name+" ha sido creado. ");
                    return RedirectToAction("ListUsers", "Administrator");
                }
            }
            Error("No se pudo crear el paquete.");
            return RedirectToAction("ListUsers", "Administrator");
        }

        [HttpGet]
        public ActionResult ListPremiumPackages()
        {
            List<PremiumPackage> listaUsuarios = _readOnlyRepository.GetAllAccounts<PremiumPackage>().ToList();
            //List<Account> listaUsuarios = _readOnlyRepository.Query<Account>(x => x.Email == "*").ToList();
            List<PremiumPackageModel> lista = new List<PremiumPackageModel>();

            for (int i = 0; i < listaUsuarios.Count; i++)
            {

                PremiumPackageModel model = new PremiumPackageModel();
                var user = listaUsuarios.ElementAt(i);
                model.Name = user.Name;
                model.Description = user.Description;
                model.Days = user.Days;
                model.Space = user.Space;
                model.Available = user.Available;
                lista.Add(model);
            }
            //var listOfContent = Builder<ListUsersModel>.CreateListOfSize(10).Build().ToList();
            return View(lista);
        }

        [HttpPost]
        public ActionResult ListPremiumPackages(PremiumPackageModel model)
        {

            return RedirectToAction("ListUsers", "Administrator");
        }

        [HttpGet]
        public ActionResult TogglePremium()
        {
            return View(new TogglePremiumPackageModel());
        }

        [HttpPost]
        public ActionResult TogglePremium(TogglePremiumPackageModel model)
        {
            var package = _readOnlyRepository.First<PremiumPackage>(c => c.Name == model.Name);
            package.Available = model.Availability;
            _writeOnlyRepository.Update(package);
            return RedirectToAction("ListPremiumPackages", "Administrator");
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
