using Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IClienteServicio
    {
        int CrearCliente(Cliente cliente);
        List<Cliente> ListarClientes();
        List<Cliente> ListarClientesConEliminados();
        Cliente EditarCliente(Cliente cliente);
        void EliminarCliente(int id);
        Cliente BuscarCliente(int id);
        List<Cliente> ListarClientesSinPedidosActivos(List<Pedido> listaPedidosActivos);
    }
}
