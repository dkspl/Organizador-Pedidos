using Entidades.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Servicios;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TPWeb3.Models;

namespace TPWeb3.Controllers
{
    public class HomeController : Controller
    {
        private IUsuarioServicio UsuarioServicio;

        public HomeController(_20211CTPContext contexto)
        {
            UsuarioServicio = new UsuarioServicio(contexto);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(UsuarioVM usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario usuarioValidado = UsuarioServicio.ValidarUsuario(usuario.Email, usuario.Password);
                if (usuarioValidado != null)
                {
                    HttpContext.Session.SetInt32("Usuario", usuarioValidado.IdUsuario);
                    return Redirect("/Cliente");
                }
                ViewBag.error = "Email y/o password incorrectos.";
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Usuario");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
