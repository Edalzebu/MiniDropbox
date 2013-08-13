using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MiniDropbox.Web.Models
{
    public class UpdateProfileModel
    {
        [DataType(DataType.EmailAddress)]    
        [Display(Name = "E-mail")]
        private string Email { get; set; }
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [Display(Name = "Apellido")]
        public string Lastname { get; set; }
        
       

    }
}