using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace API_Restaurante.Models
{
    public class Propina
    {
        public int id { get; set; }

        [ForeignKey("idMesero")]
        public int idMesero { get; set; }

        [ForeignKey("idMesa")]
        public int idMesa { get; set; }

        public int ValorPropina { get; set; }

        public string Fecha { get; set; }
    }
}
