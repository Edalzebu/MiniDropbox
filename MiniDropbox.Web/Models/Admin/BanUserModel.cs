using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MiniDropbox.Web.Models.Admin
{
    public class BanUserModel
    {
        [Display(Name = "Usuario o E-mail")]
        public string Usuario { get; set; }
        [Display(Name = "Razon de ban")]
        [DataType(DataType.MultilineText)]
        public string Razon { get; set; }
    }
}