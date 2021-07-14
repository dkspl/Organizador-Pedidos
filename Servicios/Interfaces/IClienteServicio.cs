using Entidades.Entidades;
using Entidades.Models;
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
        void EliminarCliente(int id, int eliminadoPor);
        Cliente BuscarCliente(int id);
        List<Cliente> ListarClientesSinPedidosActivos(List<Pedido> listaPedidosActivos);
        List<ClienteModel> ListarClienteModels(List<Cliente> cliente);
        List<ClienteModel> FiltrarClientesPorNombre(string filtro);
    }
}
