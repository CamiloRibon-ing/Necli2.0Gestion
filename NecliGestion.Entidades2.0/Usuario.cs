using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Entidades2._0
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellidos{ get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string ContraseñaHash { get; set; }

    }
}
