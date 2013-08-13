using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MiniDropbox.Web.Models.Admin
{
    public class SpaceManagerModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Nombre { get; set; }

        [Display(Name = "Espacio en GB")]
        public int Space { get; set; }

        [Display(Name = "Espacio utilizado")]
        public int CurrentUsedSpace { get; set; }
    }
}