using Entidades.Entidades;
using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ClienteServicio : IClienteServicio
    {
        _20211CTPContext Contexto;

        public ClienteServicio(_20211CTPContext dBContext)
        {
            Contexto = dBContext;
        }

        public Cliente BuscarCliente(int id)
        {
            Cliente clienteEncontrado = Contexto.Clientes.Find(id);
            return clienteEncontrado;
        }

        public int CrearCliente(Cliente cliente)
        {
            cliente.FechaCreacion = DateTime.Now;
            Contexto.Clientes.Add(cliente);
            int ingreso = Contexto.SaveChanges();
            return ingreso;
        }

        public Cliente EditarCliente(Cliente cliente)
        {
            Cliente clienteEncontrado = Contexto.Clientes.Find(cliente.IdCliente);
            clienteEncontrado.Nombre = cliente.Nombre;
            clienteEncontrado.Numero = cliente.Numero;
            clienteEncontrado.Telefono = cliente.Telefono;
            clienteEncontrado.Cuit = cliente.Cuit;
            clienteEncontrado.Email = cliente.Email;
            clienteEncontrado.Direccion = cliente.Direccion;
            clienteEncontrado.FechaModificacion = DateTime.Now;
            Contexto.SaveChanges();
            return clienteEncontrado;
        }

        public void EliminarCliente(int id, int eliminadoPor)
        {
            Cliente clienteEncontrado = Contexto.Clientes.Find(id);
            if (clienteEncontrado != null)
            {
                clienteEncontrado.FechaBorrado = DateTime.Now;
                clienteEncontrado.FechaModificacion = clienteEncontrado.FechaBorrado;
                Contexto.SaveChanges();
            }
        }
        public List<Cliente> ListarClientes()
        {
            List<Cliente> listaClientes = Contexto.Clientes.Where(a => !a.FechaBorrado.HasValue).ToList();
            return this.OrdenarClientesPorNombre(listaClientes);
        }
        public List<Cliente> ListarClientesConEliminados()
        {
            List<Cliente> listaClientes = Contexto.Clientes.ToList();
            return this.OrdenarClientesPorNombre(listaClientes);
        }
        public List<Cliente> OrdenarClientesPorNombre(List<Cliente> lista)
        {
            return lista.OrderBy(c => c.Nombre).ToList();
        }
        public List<Cliente> ListarClientesSinPedidosActivos(List<Pedido> listaPedidosActivos)
        {
            List<Cliente> listaClientes = this.ListarClientes();
            foreach(Pedido pedido in listaPedidosActivos)
            {
                if (listaClientes.Contains(pedido.IdClienteNavigation))
                {
                    listaClientes.Remove(pedido.IdClienteNavigation);
                }
            }
            return listaClientes;
        }

        public List<ClienteModel> ListarClienteModels(List<Cliente> clientes)
        {
            List<ClienteModel> clientesModel = new List<ClienteModel>();
            foreach(Cliente cliente in clientes)
            {
                ClienteModel nuevoCliente = new ClienteModel()
                {
                    IdCliente = cliente.IdCliente,
                    Numero = cliente.Numero,
                    Nombre = cliente.Nombre,
                    Direccion = cliente.Direccion,
                    Telefono = cliente.Telefono
                };
                clientesModel.Add(nuevoCliente);
            }

            return clientesModel;
        } 
        public List<ClienteModel> FiltrarClientesPorNombre(string filtro)
        {
            List<Cliente> listaClientes = this.ListarClientes();
            listaClientes = listaClientes.Where(c => c.Nombre.ToLower().Contains(filtro.ToLower())).ToList();
            List<ClienteModel> listaResponse = this.ListarClienteModels(listaClientes);
            return listaResponse;
        }
    }
}
