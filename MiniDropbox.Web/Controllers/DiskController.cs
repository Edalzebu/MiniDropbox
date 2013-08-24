using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using FizzWare.NBuilder;
using MiniDropbox.Data;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Disk;
using NHibernate.Mapping;

namespace MiniDropbox.Web.Controllers
{
    public class DiskController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;
        private readonly EmailHandler _emailHandler;

        public DiskController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _emailHandler = new EmailHandler();
        }
        [HttpGet]
        public ActionResult ListAllContent()
        {

            //var listOfContent = Builder<DiskContentModel>.CreateListOfSize(10).Build().ToList();
            var account = _readOnlyRepository.First<Account>(x => x.Email == User.Identity.Name);

            var rootfDir = _readOnlyRepository.First<Directories>(x => x.Id == account.RootId);
            
           
            return View(GetContentList(rootfDir));
        }
        [HttpGet]
        public ActionResult Move()
        {
            return View(new MoveModel());
        }
        [HttpPost]
        public ActionResult Move(MoveModel model)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public ActionResult Share()
        {
            return View(new ShareModel());
        }
        [HttpPost]
        public ActionResult Share(ShareModel model)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public ActionResult Delete()
        {
            return View(new DeleteModel());
        }
        [HttpPost]
        public ActionResult Delete(DeleteModel model)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return PartialView("Upload");
        }
        [HttpPost]
        public ActionResult Upload(UploadModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the fielname
                    var fileName = Path.GetFileName(file.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    file.SaveAs(path);
                }
                return RedirectToAction("ListAllContent", "Disk");

            }
            //Something bad happened
            return PartialView("Upload", model);   
        }

        public List<DiskContentModel> GetContentList(Directories dir)
        {
           
            var listOfDirectories = dir.SubFolder.ToList();
            var listOfContent = new List<DiskContentModel>();

            foreach (var dirs in listOfDirectories)
            {
                listOfContent.Add(Mapper.Map<Directories, DiskContentModel>(dirs));
            }
            return listOfContent;
        }
    }
}