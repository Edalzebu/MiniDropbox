using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models.Premium
{
    public class PremiumPackageModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [DataType(DataType.Currency,ErrorMessage = "Tiene que ser en formato de moneda")]
        public double Price { get; set; }
        [Display(Name = "Disponible")]
        public bool Available { get; set; }
        public int Space { get; set; }
        
        public int Days { get; set; }
    }
}