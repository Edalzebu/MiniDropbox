using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.Validation.Validators;

namespace MiniDropbox.Web.Models
{
    public class AccountRegisterModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Este campo es necesario")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Este campo es necesario")]
        public string LastName { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Este campo es necesario")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Error no es un email")]
        public string Email { get; set; }

        [Display(Name = "Contrasena")]
        [Required(ErrorMessage = "Este campo es necesario")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confime su contrasena")]
        [Required(ErrorMessage = "Este campo es necesario")]
        public string ConfirmPassword { get; set; }
    }
}