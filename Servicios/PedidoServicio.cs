using Entidades.Entidades;
using Entidades.Models;
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

        public Pedido CrearPedido(Pedido pedido, Cliente cliente)
        {
            pedido.IdClienteNavigation = cliente;
            pedido.IdEstado = 1;
            pedido.FechaCreacion = DateTime.Now;
            pedido.NroPedido = long.Parse(pedido.FechaCreacion.ToString("yMMddHHmm") + cliente.IdCliente.ToString());
            Contexto.Pedidos.Add(pedido);
            int ingreso = Contexto.SaveChanges();
            if (ingreso != 0)
            {
                return pedido;
            }
            return null;
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
                pedidoEncontrado.ModificadoPor = pedido.ModificadoPor;
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
                listaPedidos = this.BuscarPedidosNoEliminados(listaPedidos);
            }
            if(cliente != null)
            {
                listaPedidos = this.BuscarPedidosDeUnCliente(listaPedidos, (int)cliente);
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

        public void EliminarPedido(int idPedido, int eliminadoPor)
        {
            Pedido pedidoEncontrado = this.BuscarPedido(idPedido);
            if (pedidoEncontrado != null)
            {
                this.CerrarPedido(idPedido, eliminadoPor);
                pedidoEncontrado.FechaBorrado = DateTime.Now;
                pedidoEncontrado.BorradoPor = eliminadoPor;
                Contexto.SaveChanges();
            }      
        }

        public void CerrarPedido(int idPedido, int modificadoPor)
        {
            Pedido pedidoEncontrado = this.BuscarPedido(idPedido);
            if (pedidoEncontrado != null)
            {
                pedidoEncontrado.IdEstadoNavigation = this.BuscarEstadoPedidoPorDescripcion("Cerrado");
                pedidoEncontrado.FechaModificacion = DateTime.Now;
                pedidoEncontrado.ModificadoPor = modificadoPor;
                Contexto.SaveChanges();
            }
        }

        public void EntregarPedido(int idPedido, int modificadoPor)
        {
            Pedido pedidoEncontrado = this.BuscarPedido(idPedido);
            if (pedidoEncontrado != null)
            {
                pedidoEncontrado.IdEstadoNavigation = this.BuscarEstadoPedidoPorDescripcion("Entregado");
                pedidoEncontrado.FechaModificacion = DateTime.Now;
                pedidoEncontrado.ModificadoPor = modificadoPor;
                Contexto.SaveChanges();
            }
        }
        public EstadoPedido BuscarEstadoPedidoPorDescripcion(string valor)
        {
            return Contexto.EstadoPedidos.Where(ep => ep.Descripcion.Equals(valor)).FirstOrDefault();
        }

        public List<PedidoModel> ListarPedidoModels(int? cliente, int? estado, string incluir)
        {
            List<Pedido> listaPedidos = this.ListarPedidos(cliente, estado, incluir);
            List<PedidoModel> listaPedidosModel = new List<PedidoModel>();
            foreach (Pedido pedido in listaPedidos)
            {
                PedidoModel nuevoPedido = new PedidoModel()
                {
                    IdPedido = pedido.IdPedido,
                    IdCliente = pedido.IdCliente,
                    Estado = pedido.IdEstadoNavigation.Descripcion,
                    FechaModificacion = pedido.FechaModificacion
                };
                if (pedido.ModificadoPorNavigation != null)
                {
                    nuevoPedido.ModificadoPor = new UsuarioModel()
                    {
                        Email = pedido.ModificadoPorNavigation.Email,
                        Nombre = pedido.ModificadoPorNavigation.Nombre,
                        Apellido = pedido.ModificadoPorNavigation.Apellido,
                        IdUsuario = pedido.ModificadoPorNavigation.IdUsuario.ToString(),
                        FechaNacimiento = pedido.ModificadoPorNavigation.FechaNacimiento
                    };
                }
                
                nuevoPedido.Articulos = this.ListarArticulosConCantidades(pedido.PedidoArticulos.ToList());
                listaPedidosModel.Add(nuevoPedido);
            }
            return listaPedidosModel;
        }

        public List<ArticuloCantidadModel> ListarArticulosConCantidades(List<PedidoArticulo> articulos)
        {
            List<ArticuloCantidadModel> listaModel = new List<ArticuloCantidadModel>();
            foreach (PedidoArticulo articulo in articulos)
            {
                ArticuloCantidadModel articuloModel = new ArticuloCantidadModel()
                {
                    IdArticulo = articulo.IdArticulo,
                    Codigo = articulo.IdArticuloNavigation.Codigo,
                    Descripcion = articulo.IdArticuloNavigation.Descripcion,
                    Cantidad = articulo.Cantidad
                };
                listaModel.Add(articuloModel);
            }
            return listaModel;
        }

        public int? CrearPedidoAPI(PedidoRequestModel pedido)
        {
            Pedido nuevoPedido = new Pedido()
            {
                IdPedido = pedido.IdPedido,
                IdEstado = 1,
                ModificadoPor = pedido.ModificadoPor,
                IdCliente = pedido.IdCliente,
            };
            foreach(ArticuloCantidadModel articulo in pedido.Articulos)
            {
                PedidoArticulo pedidoArticulo = new PedidoArticulo()
                {
                    IdArticulo = articulo.IdArticulo,
                    Cantidad = articulo.Cantidad
                };
                nuevoPedido.PedidoArticulos.Add(pedidoArticulo);
            }
            nuevoPedido.FechaCreacion = DateTime.Now;
            nuevoPedido.NroPedido = long.Parse(nuevoPedido.FechaCreacion.ToString("yMMddHHmm") + pedido.IdCliente.ToString());
            return pedido.IdPedido;
        }

        public void BorrarPedidosDeClienteBorrado(int idCliente, int eliminadoPor)
        {
            List<Pedido> listaPedidos = this.BuscarPedidosNoEliminados(this.ListarTodosLosPedidos());
            listaPedidos = this.BuscarPedidosDeUnCliente(listaPedidos, idCliente);

            foreach(Pedido pedido in listaPedidos)
            {
                this.EliminarPedido(pedido.IdPedido, eliminadoPor);
            }
        }
        public List<Pedido> BuscarPedidosDeUnCliente(List<Pedido> pedidos, int IdCliente)
        {
            return pedidos.Where(p => p.IdCliente == IdCliente).ToList();
        }
        public List<Pedido> BuscarPedidosNoEliminados(List<Pedido> pedidos)
        {
            return pedidos.Where(p => !p.FechaBorrado.HasValue).ToList();
        }
        public void BorrarArticulosPorArticuloBorrado(int idArticulo, int eliminadoPor)
        {
            List<PedidoArticulo> lista = this.BuscarPedidosSegunArticulo(idArticulo);
            foreach(PedidoArticulo pedido in lista)
            {
                Contexto.PedidoArticulos.Remove(pedido);
                Contexto.SaveChanges();
                Pedido pedidoModificado = this.BuscarPedido(pedido.IdPedido);
                pedidoModificado.ModificadoPor = eliminadoPor;
                this.EditarPedido(pedidoModificado);
            }    
        }

        public List<PedidoArticulo> BuscarPedidosSegunArticulo(int idArticulo)
        {
            List<PedidoArticulo> lista = Contexto.PedidoArticulos.Where(pa => pa.IdArticulo == idArticulo).ToList();
            return lista;
        }
    }
}
