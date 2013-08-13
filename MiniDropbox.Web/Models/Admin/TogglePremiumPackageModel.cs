using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models.Admin
{
    public class TogglePremiumPackageModel
    {
        [Display(Name = "Nombre del Paquete")]
        public string Name { get; set; }
        [Display(Name = "Disponible")]
        public bool Availability { get; set; }
    }
}