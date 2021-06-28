using Entidades.Entidades;
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
        public int CrearArticulo(Articulo articulo)
        {
            articulo.FechaCreacion = DateTime.Now;
            Contexto.Articulos.Add(articulo);
            int ingreso = Contexto.SaveChanges();
            return ingreso;
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

        public void EliminarArticulo(int id)
        {
            Articulo articuloEncontrado = Contexto.Articulos.Find(id);
            if (articuloEncontrado != null)
            {
                articuloEncontrado.FechaBorrado = DateTime.Now;
                articuloEncontrado.FechaModificacion = articuloEncontrado.FechaBorrado;
                Contexto.SaveChanges();
            }
        }

        public List<Articulo> ListarArticulos()
        {
            return Contexto.Articulos.Where(a => !a.FechaBorrado.HasValue).ToList();
        }
        public List<Articulo> ListarArticulosConEliminados()
        {
            return Contexto.Articulos.ToList();
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
    }
}
