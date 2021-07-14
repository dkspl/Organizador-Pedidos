using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Models
{
    public class PedidoModel
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public UsuarioModel ModificadoPor { get; set; }
        public List<ArticuloCantidadModel> Articulos { get; set; }

    }
}
