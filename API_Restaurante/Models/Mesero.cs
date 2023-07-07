using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Restaurante.Models
{
    public class Mesero
    {
        public int id { get; set; } 
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public int Cedula { get; set; }
    }
}
