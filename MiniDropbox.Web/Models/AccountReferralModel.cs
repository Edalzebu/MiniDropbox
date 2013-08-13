using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MiniDropbox.Web.Models
{
    public class AccountReferralModel
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "E-Mail invalido")]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
    }
}