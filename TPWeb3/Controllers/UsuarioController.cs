using Entidades.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using Servicios.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TPWeb3.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private IUsuarioServicio _ServicioUsuario;

        public UsuarioController(_20211CTPContext context, IJwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor)
        {
            _ServicioUsuario = new UsuarioServicio(context, jwtHelper,httpContextAccessor);
        }

        [HttpGet]
        public IActionResult Index(string incluir)
        {
            ViewBag.Incluir = incluir;
            if (!string.IsNullOrEmpty(incluir) && incluir.Equals("on"))
            {
                return View(_ServicioUsuario.ListarUsuariosConEliminados());
            }
            return View(_ServicioUsuario.ListarUsuarios());

        }

        public IActionResult Detalle(int id)
        {
            Usuario usuarioEncontrado = _ServicioUsuario.BuscarUsuario(id);
            return View(usuarioEncontrado);
        }

        public IActionResult NuevoUsuario()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NuevoUsuario(Usuario usuario, int retorno)
        {
            if (ModelState.IsValid)
            {
                usuario.CreadoPor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
                if (_ServicioUsuario.CrearUsuario(usuario) == 1)
                {
                    if (retorno == 0)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("NuevoUsuario");
                }
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuario)
        {
            usuario.ModificadoPor= Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
            _ServicioUsuario.EditarUsuario(usuario);
            return Redirect("/Usuario/Detalle/" + usuario.IdUsuario);
        }
        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            int eliminadoPor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
            _ServicioUsuario.EliminarUsuario(id, eliminadoPor);
            return RedirectToAction("Index");
        }
    }
}
