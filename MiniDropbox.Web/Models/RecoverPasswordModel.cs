using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniDropbox.Web.Models
{
    public class RecoverPasswordModel
    {
        [Display(Name = "E-mail")]
        [HiddenInput(DisplayValue = true)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string Password { get; set; }
        [Display(Name = "Confimar Contraseña")]
        [DataType(DataType.Password)]
        public string Confirmpassword { get; set; }
    }
}