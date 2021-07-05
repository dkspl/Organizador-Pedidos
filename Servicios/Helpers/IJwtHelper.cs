using Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Helpers
{
    public interface IJwtHelper
    {
        public string GenerarToken(Usuario usuario);
    }
}
