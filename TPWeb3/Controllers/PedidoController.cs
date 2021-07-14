using AspNetCoreHero.ToastNotification.Abstractions;
using Entidades.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TPWeb3.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private IPedidoServicio PedidoServicio;
        private IArticuloServicio ArticuloServicio;
        private IClienteServicio ClienteServicio;
        private readonly INotyfService _notyf;

        public PedidoController(_20211CTPContext contexto, INotyfService notyf)
        {
            PedidoServicio = new PedidoServicio(contexto);
            ArticuloServicio = new ArticuloServicio(contexto);
            ClienteServicio = new ClienteServicio(contexto);
            _notyf = notyf;
        }
        public IActionResult Index(int? cliente, int? estado, string incluir)
        {
            ViewBag.Clientes = ClienteServicio.ListarClientes();
            ViewBag.Estados = PedidoServicio.ListarEstadosPedido();
            ViewBag.Cliente = cliente;
            if (estado == null)
                estado = 1;
            ViewBag.Estado = estado;
            ViewBag.Incluir = incluir;
            if (TempData["notificacion"] != null)
            {
                _notyf.Success("El pedido " + TempData["notificacion"] + " se ha creado con éxito.");
            }
            return View(PedidoServicio.ListarPedidos(cliente, estado, incluir));
        }
        public IActionResult NuevoPedido()
        {
            ViewBag.Clientes = ClienteServicio.ListarClientesSinPedidosActivos
                (PedidoServicio.ListarPedidosAbiertos(PedidoServicio.ListarTodosLosPedidos()));
            ViewBag.Articulos = ArticuloServicio.ListarArticulos();
            ViewBag.ErrorCliente = TempData["ErrorCliente"];
            ViewBag.ErrorArticulo = TempData["ErrorArticulo"];
            if (TempData["notificacion"] != null)
            {
                _notyf.Success("El pedido " + TempData["notificacion"] + " se ha creado con éxito.");
            }
            return View();
        }
        [HttpPost]
        public IActionResult NuevoPedido(Pedido pedido, int idCliente, Dictionary<int, int> Articulos, int retorno)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(idCliente.ToString()) &&
                     ClienteServicio.BuscarCliente(idCliente) != null)
                {
                    List<PedidoArticulo> listaArticulos = ArticuloServicio.AgregarArticulos(Articulos);
                    if (listaArticulos.Count != 0)
                    {
                        pedido.PedidoArticulos = ArticuloServicio.AgregarArticulos(Articulos);
                        pedido.CreadoPor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
                        Pedido nuevoPedido = PedidoServicio.CrearPedido(pedido, ClienteServicio.BuscarCliente(idCliente));
                        if(nuevoPedido != null)
                        {
                            TempData["notificacion"] = nuevoPedido.IdClienteNavigation.Nombre + " #" + nuevoPedido.IdPedido.ToString();
                            if (retorno == 0)
                                return RedirectToAction("Index");
                            else
                                return RedirectToAction("NuevoPedido");
                        }    
                    }
                    else
                    {
                        TempData["ErrorCliente"] = "Seleccione artículos válidos";
                    }
                }
                TempData["ErrorCliente"] = "Seleccione un cliente válido";
            }
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
                pedido.ModificadoPor=Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
                PedidoServicio.EditarPedido(pedido);
            }
            else
            {
                TempData["ErrorArticulo"] = "Seleccione artículos válidos";
                return Redirect("/Pedido/Detalle/" + pedido.IdPedido);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarPedido(int id)
        {
            int eliminadoPor= Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
            PedidoServicio.EliminarPedido(id, eliminadoPor);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CerrarPedido(int id)
        {
            int modificadoPor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
            PedidoServicio.CerrarPedido(id, modificadoPor);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EntregarPedido(int id)
        {
            int modificadoPor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
            PedidoServicio.EntregarPedido(id, modificadoPor);
            return RedirectToAction("Index");
        }
    }
}
