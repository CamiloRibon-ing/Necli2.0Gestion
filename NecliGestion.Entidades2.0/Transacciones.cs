using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Entidades2._0
{
    public class Transacciones
    {

        public Guid NumeroTransaccion {get; set;}

        public int NumeroCuentaOrigen { get; set; }
        public int NumeroCuentaDestino { get; set; }
        public decimal Monto { get; set; }

        public string Tipo { get; set; }

        public DateTime Fecha { get; set; }
    }
}
