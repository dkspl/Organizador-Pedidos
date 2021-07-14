using Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class PedidoServicio : IPedidoServicio
    {
        _20211CTPContext Contexto;

        public PedidoServicio(_20211CTPContext dBContext)
        {
            Contexto = dBContext;
        }

        public int CrearPedido(Pedido pedido, Cliente cliente)
        {
            pedido.IdClienteNavigation = cliente;
            pedido.IdEstado = 1;
            pedido.FechaCreacion = DateTime.Now;
            pedido.NroPedido = long.Parse(pedido.FechaCreacion.ToString("yMMddHHmm") + cliente.IdCliente.ToString());
            Contexto.Pedidos.Add(pedido);
            int ingreso = Contexto.SaveChanges();
            return ingreso;
        }
        public Pedido BuscarPedido(int id)
        {
            try
            {
                return Contexto.Pedidos.Where(p => p.IdPedido == id).First();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<EstadoPedido> ListarEstadosPedido()
        {
            return Contexto.EstadoPedidos.ToList();
        }

        public void EditarPedido(Pedido pedido)
        {
            Pedido pedidoEncontrado = Contexto.Pedidos.Find(pedido.IdPedido);
            if (pedidoEncontrado.IdEstadoNavigation.Descripcion.ToLower() == "abierto")
            {
                pedidoEncontrado.IdCliente = pedido.IdCliente;
                pedidoEncontrado.Comentarios = pedido.Comentarios;
                pedidoEncontrado.FechaModificacion = DateTime.Now;
                pedidoEncontrado.PedidoArticulos = this.EditarArticulosDeUnPedido(pedido.PedidoArticulos.ToList(), pedido.IdPedido);
                Contexto.SaveChanges();
            }
        }

        public List<PedidoArticulo> EditarArticulosDeUnPedido(List<PedidoArticulo> listaArticulos, int idPedido)
        {
            List<PedidoArticulo> pedidosArticulos = this.BuscarArticulosDeUnPedido(idPedido);
            foreach (var articuloNuevo in listaArticulos)
            {
                int flag = 0;
                foreach (var articuloDB in pedidosArticulos)
                {
                    if (articuloDB.IdArticulo == articuloNuevo.IdArticulo)
                    {
                        articuloDB.Cantidad = articuloNuevo.Cantidad;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    pedidosArticulos.Add(articuloNuevo);
                }
            }
            return pedidosArticulos;
        }
        public List<PedidoArticulo> BuscarArticulosDeUnPedido(int id)
        {
            return Contexto.PedidoArticulos.Where(pa => pa.IdPedido == id).ToList();
        }


        public List<Pedido> OrdenarPedidosPorCodigo(List<Pedido> lista)
        {
            return lista.OrderBy(p => p.IdPedido).ToList();
        }
        public List<Pedido> OrdenarPedidosPorCreacionReciente(List<Pedido> lista)
        {
            return lista.OrderBy(p => p.FechaCreacion).ToList();
        }
        public EstadoPedido BuscarEstadoPedidoPorId(int idPedido)
        {
            EstadoPedido estado = Contexto.EstadoPedidos.Find(idPedido);
            return estado;
        }

        public List<Pedido> ListarTodosLosPedidos()
        {
            List<Pedido> listaPedidos = Contexto.Pedidos.ToList();
            return this.OrdenarPedidosPorCreacionReciente(listaPedidos);
        }
        public List<Pedido> ListarPedidosAbiertos(List<Pedido> listaPedidos)
        {
            List<Pedido> listaAbiertos = listaPedidos.
                Where(p => p.IdEstadoNavigation.Descripcion.ToLower().Equals("abierto"))
                .ToList();
            return listaAbiertos;
        }
        public List<Pedido> ListarPedidos(int? cliente, int? estado, string incluir)
        {
            List<Pedido> listaPedidos = this.ListarTodosLosPedidos();
            if(string.IsNullOrEmpty(incluir) || !incluir.Equals("on"))
            {
                listaPedidos = listaPedidos.Where(p => !p.FechaBorrado.HasValue).ToList();
            }
            if(cliente != null)
            {
                listaPedidos = listaPedidos.Where(p => p.IdCliente == cliente).ToList();
            }
            if(estado != null)
            {
                if(estado != 0)
                    listaPedidos = listaPedidos.Where(p => p.IdEstado == estado).ToList();
            }
            else
            {
                listaPedidos = this.ListarPedidosAbiertos(listaPedidos);
            }
            return listaPedidos;
        }

        public void EliminarPedido(int idPedido)
        {
            Pedido pedidoEncontrado = this.BuscarPedido(idPedido);
            if (pedidoEncontrado != null)
            {
                pedidoEncontrado.FechaBorrado = DateTime.Now;
                Contexto.SaveChanges();
            }      
        }

        public void CerrarPedido(int idPedido)
        {
            Pedido pedidoEncontrado = this.BuscarPedido(idPedido);
            if (pedidoEncontrado != null)
            {
                pedidoEncontrado.IdEstadoNavigation = this.BuscarEstadoPedidoPorDescripcion("Cerrado");
                Contexto.SaveChanges();
            }
        }

        public void EntregarPedido(int idPedido)
        {
            Pedido pedidoEncontrado = this.BuscarPedido(idPedido);
            if (pedidoEncontrado != null)
            {
                pedidoEncontrado.IdEstadoNavigation = this.BuscarEstadoPedidoPorDescripcion("Entregado");
                Contexto.SaveChanges();
            }
        }
        public EstadoPedido BuscarEstadoPedidoPorDescripcion(string valor)
        {
            return Contexto.EstadoPedidos.Where(ep => ep.Descripcion.Equals(valor)).FirstOrDefault();
        }
    }
}
