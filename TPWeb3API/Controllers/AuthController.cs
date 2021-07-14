using Entidades;
using Entidades.Entidades;
using Entidades.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using Servicios.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPWeb3.Models;

namespace TPWeb3API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private IUsuarioServicio UsuarioServicio;

        public AuthController(_20211CTPContext contexto, IJwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor)
        {
            UsuarioServicio = new UsuarioServicio(contexto, jwtHelper, httpContextAccessor);
        }
        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public UsuarioLogueadoModel Login(UsuarioVM usuario)
        {
            UsuarioLogueadoModel usuarioValidado = UsuarioServicio.IniciarSesionAPI(usuario.Email, usuario.Password);
            return usuarioValidado;
        }
    }
}
