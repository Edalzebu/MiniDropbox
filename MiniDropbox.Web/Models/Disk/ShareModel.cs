using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MiniDropbox.Domain.Entities;

namespace MiniDropbox.Web.Models.Disk
{
    public class ShareModel
    {
        [Display(Name = "Nombre")]
        public string FileName { get; set; }
        public List<Account> ShareWith { get; set; }
    
    }
}