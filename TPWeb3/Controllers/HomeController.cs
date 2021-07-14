using AspNetCoreHero.ToastNotification.Abstractions;
using Entidades;
using Entidades.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Servicios;
using Servicios.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TPWeb3.Models;

namespace TPWeb3.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IUsuarioServicio UsuarioServicio;
        private readonly INotyfService _notyf;

        public HomeController(_20211CTPContext contexto, IJwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor, INotyfService notyf)
        {
            UsuarioServicio = new UsuarioServicio(contexto, jwtHelper,httpContextAccessor);
            _notyf = notyf;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Pedido");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(UsuarioVM usuario)
        {
            if (ModelState.IsValid)
            {
                UsuarioResponseModel usuarioValidado=UsuarioServicio.IniciarSesion(usuario.Email, usuario.Password);
                if (usuarioValidado != null)
                {
                    return Redirect("/Pedido");
                }
                ViewBag.error = "Email y/o password incorrectos.";
            }
            return View();
        }
        public IActionResult Logout()
        {
            UsuarioServicio.CerrarSesion();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NoAutorizado()
        {
            return View();
        }
    }
}
