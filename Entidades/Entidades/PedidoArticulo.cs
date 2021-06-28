using System;
using System.Collections.Generic;

#nullable disable

namespace Entidades.Entidades
{
    public partial class PedidoArticulo
    {
        public int IdPedido { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }

        public virtual Articulo IdArticuloNavigation { get; set; }
        public virtual Pedido IdPedidoNavigation { get; set; }
    }
}
