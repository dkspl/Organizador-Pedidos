using Entidades.Entidades;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPWeb3.Controllers
{
    public class ArticuloController : Controller
    {
        private IArticuloServicio ArticuloServicio;

        public ArticuloController(_20211CTPContext contexto)
        {
            ArticuloServicio = new ArticuloServicio(contexto);
        }
        public IActionResult Index(string incluir)
        {
            ViewBag.Incluir = incluir;
            if (!string.IsNullOrEmpty(incluir) && incluir.Equals("on"))
            {
                return View(ArticuloServicio.ListarArticulosConEliminados());
            }
            return View(ArticuloServicio.ListarArticulos());
        }
        public IActionResult NuevoArticulo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CrearArticulo(Articulo articulo, int retorno)
        {
            if (ArticuloServicio.CrearArticulo(articulo) == 1)
            {
                if (retorno == 0)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("NuevoArticulo");
            }
            return View(articulo);
        }

        public IActionResult Detalle(int id)
        {
            Articulo articuloEncontrado = ArticuloServicio.BuscarArticulo(id);
            return View(articuloEncontrado);
        }
        [HttpPost]
        public IActionResult EliminarArticulo(int id)
        {
            ArticuloServicio.EliminarArticulo(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditarArticulo(Articulo articulo)
        {
            ArticuloServicio.EditarArticulo(articulo);
            return Redirect("/Articulo/Detalle/" + articulo.IdArticulo);
        }
    }
}
