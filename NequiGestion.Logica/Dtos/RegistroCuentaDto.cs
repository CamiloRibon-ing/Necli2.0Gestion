using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Logica.Dtos
{
    public record RegistroCuentaDto
    {
        public int NumeroCuenta { get; set; }
        public int IdUsuario { get; set; }
        public decimal Saldo { get; set; }
    }
}
