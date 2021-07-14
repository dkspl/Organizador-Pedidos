using Entidades.Entidades;
using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ArticuloServicio : IArticuloServicio
    {
        _20211CTPContext Contexto;

        public ArticuloServicio(_20211CTPContext dBContext)
        {
            Contexto = dBContext;
        }
        public Articulo CrearArticulo(Articulo articulo)
        {
            articulo.FechaCreacion = DateTime.Now;
            Contexto.Articulos.Add(articulo);
            int ingreso = Contexto.SaveChanges();
            if (ingreso == 1)
                return articulo;
            return null;
        }

        public Articulo EditarArticulo(Articulo articulo)
        {
            Articulo articuloEncontrado = Contexto.Articulos.Find(articulo.IdArticulo);
            articuloEncontrado.Codigo = articulo.Codigo;
            articuloEncontrado.Descripcion = articulo.Descripcion;
            articuloEncontrado.FechaModificacion = DateTime.Now;
            Contexto.SaveChanges();
            return articuloEncontrado;
        }

        public void EliminarArticulo(int id, int eliminadoPor)
        {
            Articulo articuloEncontrado = Contexto.Articulos.Find(id);
            if (articuloEncontrado != null)
            {
                articuloEncontrado.FechaBorrado = DateTime.Now;
                articuloEncontrado.BorradoPor = eliminadoPor;
                Contexto.SaveChanges();
            }
        }

        public List<Articulo> ListarArticulos()
        {
            List<Articulo> listaArticulos = Contexto.Articulos.Where(a => !a.FechaBorrado.HasValue).ToList();
            listaArticulos = this.OrdenarArticulosPorCodigo(listaArticulos);
            return listaArticulos;
        }
        public List<Articulo> ListarArticulosConEliminados()
        {
            List<Articulo> listaArticulos = Contexto.Articulos.ToList();
            listaArticulos = this.OrdenarArticulosPorCodigo(listaArticulos);
            return listaArticulos;
        }
        public Articulo BuscarArticulo(int id)
        {
            Articulo articuloEncontrado = Contexto.Articulos.Find(id);
            return articuloEncontrado;
        }

        public List<PedidoArticulo> AgregarArticulos(Dictionary<int, int> articulos)
        {
            List<PedidoArticulo> articulosConCantidad = new List<PedidoArticulo>();
            foreach (var item in articulos)
            {
                Articulo articulo = this.BuscarArticulo(item.Key);
                if (articulo != null)
                {
                    articulosConCantidad.Add(new PedidoArticulo()
                    {
                        IdArticuloNavigation = articulo,
                        IdArticulo = articulo.IdArticulo,
                        Cantidad = item.Value
                    });
                }
            };
            return articulosConCantidad;
        }
        public List<Articulo> OrdenarArticulosPorCodigo(List<Articulo> lista)
        {
            List<Articulo> listaOrdenada = lista.OrderBy(a => a.Codigo).ToList();
            return listaOrdenada;
        }

        public List<ArticuloModel> ListarArticuloModels(List<Articulo> articulos)
        {
            List<ArticuloModel> articulosModel = new List<ArticuloModel>();
            foreach (Articulo articulo in articulos)
            {
                ArticuloModel nuevoCliente = new ArticuloModel()
                {
                    IdArticulo = articulo.IdArticulo,
                    Codigo = articulo.Codigo,
                    Descripcion = articulo.Descripcion
                };
                articulosModel.Add(nuevoCliente);
            }

            return articulosModel;
        }

        public List<ArticuloModel> FiltrarArticulosPorNombre(string filtro)
        {
            List<Articulo> listaArticulos = this.ListarArticulos();
            listaArticulos = listaArticulos.Where(c => c.Descripcion.ToLower().Contains(filtro.ToLower())).ToList();
            List<ArticuloModel> listaResponse = this.ListarArticuloModels(listaArticulos);
            return listaResponse;
        }
    }
}
