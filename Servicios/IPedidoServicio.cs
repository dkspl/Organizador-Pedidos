using Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IPedidoServicio
    {
        int CrearPedido(Pedido pedido, Cliente cliente);
        Pedido BuscarPedido(int id);
        List<EstadoPedido> ListarEstadosPedido();
        void EditarPedido(Pedido pedido);
        List<PedidoArticulo> BuscarArticulosDeUnPedido(int id);
        List<PedidoArticulo> EditarArticulosDeUnPedido(List<PedidoArticulo> listaArticulos, int idPedido);
        List<Pedido> ListarTodosLosPedidos();
        List<Pedido> ListarPedidosAbiertos(List<Pedido> listaPedidos);
        void EliminarPedido(int idPedido);
        void CerrarPedido(int idPedido);
        void EntregarPedido(int idPedido);
        List<Pedido> ListarPedidos(int? cliente, int? estado, string incluir);
    }
}
