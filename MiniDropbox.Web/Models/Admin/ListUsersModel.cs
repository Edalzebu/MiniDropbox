using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MiniDropbox.Web.Models.Admin
{
    public class ListUsersModel
    {
        public int id { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail no valido")]
        public string email { get; set; }
        [Display(Name = "Espacio Disponible")]
        public int availableSpace { get; set; }
    }
}