using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyf;

        public ClienteController(_20211CTPContext contexto, INotyfService notyf)
        {
            ClienteServicio = new ClienteServicio(contexto);
            _notyf = notyf;
        }
        public IActionResult Index(string incluir)
        {
            ViewBag.Incluir = incluir;
            if (TempData["notificacion"] != null)
            {
                _notyf.Success("El cliente " + TempData["notificacion"] + " se ha creado con éxito.");
            }
            if (!string.IsNullOrEmpty(incluir) && incluir.Equals("on"))
            {
                return View(ClienteServicio.ListarClientesConEliminados());
            }
            return View(ClienteServicio.ListarClientes());
        }
        public IActionResult NuevoCliente()
        {
            if(TempData["notificacion"] != null)
            {
                _notyf.Success("El cliente " + TempData["notificacion"] + " se ha creado con éxito.");
            }
            return View();
        }
        [HttpPost]
        public IActionResult NuevoCliente(Cliente cliente, int retorno)
        {
            if (ModelState.IsValid)
            {
                if (ClienteServicio.CrearCliente(cliente) == 1)
                {
                    TempData["notificacion"] = cliente.Nombre;
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
