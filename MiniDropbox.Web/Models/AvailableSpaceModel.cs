using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MiniDropbox.Web.Models
{
    public class AvailableSpaceModel
    {
        public int EspacioTotal { get; set; }
        public int EspacioUsado { get; set; }
        public int EspacioDisponible { get; set; }
    }
}