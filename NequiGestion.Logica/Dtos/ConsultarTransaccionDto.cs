using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Logica.Dtos
{
    public class ConsultarTransaccionDto
    {


        public string NumeroTransaccion { get; set; }
        public DateTime Desde { get; set; }

        public DateTime Hasta { get; set; }

        public string NumeroCuentaOrigen { get; set;}
        public string NumeroCuentaDestino { get; set; }


    }
}
