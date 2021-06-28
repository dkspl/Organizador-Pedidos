using System;
using System.Collections.Generic;

#nullable disable

namespace Entidades.Entidades
{
    public partial class Pedido
    {
        public Pedido()
        {
            PedidoArticulos = new HashSet<PedidoArticulo>();
        }

        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public int IdEstado { get; set; }
        public long NroPedido { get; set; }
        public string Comentarios { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
        public int? BorradoPor { get; set; }

        public virtual Usuario BorradoPorNavigation { get; set; }
        public virtual Usuario CreadoPorNavigation { get; set; }
        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual EstadoPedido IdEstadoNavigation { get; set; }
        public virtual Usuario ModificadoPorNavigation { get; set; }
        public virtual ICollection<PedidoArticulo> PedidoArticulos { get; set; }
    }
}
