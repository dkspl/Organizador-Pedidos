using Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IArticuloServicio
    {
        int CrearArticulo(Articulo articulo);
        List<Articulo> ListarArticulos();
        List<Articulo> ListarArticulosConEliminados();
        Articulo EditarArticulo(Articulo articulo);
        void EliminarArticulo(int id);
        Articulo BuscarArticulo(int id);
        List<PedidoArticulo> AgregarArticulos(Dictionary<int, int> articulos);
    }
}
