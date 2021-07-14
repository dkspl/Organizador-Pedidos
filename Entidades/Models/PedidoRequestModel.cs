using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Models
{
    public class PedidoRequestModel
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public int ModificadoPor { get; set; }
        public List<ArticuloCantidadModel> Articulos { get; set; }

    }
}
