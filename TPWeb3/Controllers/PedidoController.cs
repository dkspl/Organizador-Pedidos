using Entidades.Entidades;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPWeb3.Controllers
{
    public class PedidoController : Controller
    {
        private IPedidoServicio PedidoServicio;
        private IArticuloServicio ArticuloServicio;
        private IClienteServicio ClienteServicio;

        public PedidoController(_20211CTPContext contexto)
        {
            PedidoServicio = new PedidoServicio(contexto);
            ArticuloServicio = new ArticuloServicio(contexto);
            ClienteServicio = new ClienteServicio(contexto);
        }
        public IActionResult Index()
        {
            return View(PedidoServicio.ListarPedidosAbiertos());
        }
        public IActionResult NuevoPedido()
        {
            ViewBag.Clientes = ClienteServicio.ListarClientesSinPedidosActivos(PedidoServicio.ListarPedidosActivos());
            ViewBag.Articulos = ArticuloServicio.ListarArticulos();
            ViewBag.ErrorCliente = TempData["ErrorCliente"];
            ViewBag.ErrorArticulo = TempData["ErrorArticulo"];
            return View();
        }
        [HttpPost]
        public IActionResult NuevoPedido(Pedido pedido, int idCliente, Dictionary<int, int> Articulos, int retorno)
        {
            /*if (ModelState.IsValid)
            {*/
            if (!string.IsNullOrEmpty(idCliente.ToString()) &&
                 ClienteServicio.BuscarCliente(idCliente) != null)
            {
                List<PedidoArticulo> listaArticulos = ArticuloServicio.AgregarArticulos(Articulos);
                if (listaArticulos.Count != 0)
                {
                    pedido.PedidoArticulos = ArticuloServicio.AgregarArticulos(Articulos);
                    PedidoServicio.CrearPedido(pedido, ClienteServicio.BuscarCliente(idCliente));
                    if (retorno == 0)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("NuevoPedido");
                }
                else
                {
                    TempData["ErrorCliente"] = "Seleccione artículos válidos";
                }
            }
            TempData["ErrorCliente"] = "Seleccione un cliente válido";
            //}
            return RedirectToAction("NuevoPedido");
        }
        public IActionResult Detalle(int id)
        {
            Pedido pedidoEncontrado = PedidoServicio.BuscarPedido(id);
            if (pedidoEncontrado != null)
            {
                ViewBag.Clientes = ClienteServicio.ListarClientes();
                ViewBag.Articulos = ArticuloServicio.ListarArticulos();
                ViewBag.ErrorArticulo = TempData["ErrorArticulo"];
                ViewBag.EstadoPedido = PedidoServicio.ListarEstadosPedido();
                return View(pedidoEncontrado);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditarPedido(Pedido pedido, Dictionary<int, int> Articulos)
        {
            List<PedidoArticulo> listaArticulos = ArticuloServicio.AgregarArticulos(Articulos);
            if (listaArticulos.Count != 0)
            {
                pedido.PedidoArticulos = ArticuloServicio.AgregarArticulos(Articulos);
                PedidoServicio.EditarPedido(pedido);
            }
            else
            {
                TempData["ErrorArticulo"] = "Seleccione artículos válidos";
                return Redirect("/Pedido/Detalle/" + pedido.IdPedido);
            }
            return RedirectToAction("Index");
        }

        public IActionResult TestDic()
        {
            ViewBag.Articulos = ArticuloServicio.ListarArticulos();
            return View();
        }
    }
}
