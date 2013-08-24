using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniDropbox.Web.Models.Disk
{
    public class DeleteModel
    {
        [HiddenInput(DisplayValue = true)]
        [Display(Name = "Nombre Archivo")]
        public string FileName { get; set; }
        [HiddenInput(DisplayValue = true)]
        [Display(Name = "Directorio Actual")]
        public string Directory { get; set; }
    }
}