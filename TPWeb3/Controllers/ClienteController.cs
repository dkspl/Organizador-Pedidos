using Entidades.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPWeb3.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClienteController : Controller
    {
        private IClienteServicio ClienteServicio;

        public ClienteController(_20211CTPContext contexto)
        {
            ClienteServicio = new ClienteServicio(contexto);
        }
        public IActionResult Index(string incluir)
        {
            ViewBag.Incluir = incluir;
            if (!string.IsNullOrEmpty(incluir) && incluir.Equals("on"))
            {
                return View(ClienteServicio.ListarClientesConEliminados());
            }
            return View(ClienteServicio.ListarClientes());
        }
        public IActionResult NuevoCliente()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NuevoCliente(Cliente cliente, int retorno)
        {
            if (ModelState.IsValid)
            {
                if (ClienteServicio.CrearCliente(cliente) == 1)
                {
                    if (retorno == 0)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("NuevoCliente");
                }
            }
            return View(cliente);
        }

        public IActionResult Detalle(int id)
        {
            Cliente clienteEncontrado = ClienteServicio.BuscarCliente(id);
            return View(clienteEncontrado);
        }
        [HttpPost]
        public IActionResult EliminarCliente(int id)
        {
            ClienteServicio.EliminarCliente(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditarCliente(Cliente cliente)
        {
            ClienteServicio.EditarCliente(cliente);
            return Redirect("/Cliente/Detalle/" + cliente.IdCliente);
        }
    }
}
