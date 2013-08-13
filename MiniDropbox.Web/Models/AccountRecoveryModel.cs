using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MiniDropbox.Web.Models
{
    public class AccountRecoveryModel
    {
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-Mail invalido")]
        public string Email { get; set; }
    }
}