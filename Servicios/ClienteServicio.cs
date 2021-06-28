using Entidades.Entidades;
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

        public void EliminarCliente(int id)
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
            return Contexto.Clientes.Where(a => !a.FechaBorrado.HasValue).ToList();
        }
        public List<Cliente> ListarClientesConEliminados()
        {
            return Contexto.Clientes.ToList();
        }
    }
}
