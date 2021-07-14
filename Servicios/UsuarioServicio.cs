using Entidades;
using Entidades.Entidades;
using Entidades.Models;
using Microsoft.AspNetCore.Http;
using Servicios.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        _20211CTPContext _contexto;
        IJwtHelper JwtHelper;
        IHttpContextAccessor _httpContextAccessor;

        public UsuarioServicio(_20211CTPContext dBContext, IJwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor)
        {
            _contexto = dBContext;
            JwtHelper = jwtHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Usuario ValidarUsuario(string email, string password)
        {
            Usuario usuarioEncontrado = this.BuscarPorEmail(email);
            if (usuarioEncontrado != null)
            {
                if (usuarioEncontrado.Password.Equals(this.HashSHA256(password)))
                    return usuarioEncontrado;
            }
            return null;
        }

        public UsuarioResponse IniciarSesion(string email, string password)
        {
            Usuario usuarioEncontrado = this.ValidarUsuario(email, password);
            if(usuarioEncontrado!= null)
            {
                UsuarioResponse usuario = new UsuarioResponse()
                {
                    email = usuarioEncontrado.Email,
                    id = usuarioEncontrado.IdUsuario.ToString(),
                    rol = usuarioEncontrado.EsAdmin ? "Administrador" : "Usuario"
                };
                AuthenticationHelper.SignIn(_httpContextAccessor.HttpContext, usuario);
                this.ActualizarFechaUltLogin(usuarioEncontrado.IdUsuario);
                return usuario;
            }
            return null;
        }
        public UsuarioModel IniciarSesionAPI(string email, string password)
        {
            Usuario usuarioEncontrado = this.ValidarUsuario(email, password);
            if (usuarioEncontrado != null)
            {
                UsuarioModel usuario = new UsuarioModel()
                {
                    Email = usuarioEncontrado.Email,
                    IdUsuario = usuarioEncontrado.IdUsuario.ToString(),
                    Nombre = usuarioEncontrado.Nombre,
                    Apellido = usuarioEncontrado.Apellido,
                    FechaNacimiento = usuarioEncontrado.FechaNacimiento,
                    Token = JwtHelper.GenerarToken(usuarioEncontrado)
                };
                return usuario;
            }
            return null;
        }

        public Usuario BuscarPorEmail(string email)
        {
            string emailLower = email.ToLower();
            try
            {
                Usuario usuarioEncontrado = _contexto.Usuarios.Where(u => u.Email == emailLower).First();
                return usuarioEncontrado;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public Usuario BuscarUsuario(int id)
        {
            Usuario usuarioEncontrado = _contexto.Usuarios.Find(id);
            return usuarioEncontrado;
        }
        public void ActualizarFechaUltLogin(int idUsuario)
        {
            Usuario usuarioEncontrado = this.BuscarUsuario(idUsuario);
            usuarioEncontrado.FechaUltLogin = DateTime.Now;
            _contexto.SaveChanges();
        }
        public Usuario EditarUsuario(Usuario usuario)
        {
            Usuario usuarioEncontrado = _contexto.Usuarios.Find(usuario.IdUsuario);
            usuarioEncontrado.Nombre = usuario.Nombre;
            usuarioEncontrado.Apellido = usuario.Apellido;
            usuarioEncontrado.Email = usuario.Email;
            usuarioEncontrado.EsAdmin = usuario.EsAdmin;
            usuarioEncontrado.FechaModificacion = DateTime.Now;
            _contexto.SaveChanges();
            return usuarioEncontrado;
        }

        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> listaUsuarios = _contexto.Usuarios.Where(u => !u.FechaBorrado.HasValue).ToList();
            return this.OrdenarUsuariosPorApellido(listaUsuarios);
        }
        public List<Usuario> ListarUsuariosConEliminados()
        {
            List<Usuario> listaUsuarios = _contexto.Usuarios.ToList();
            return this.OrdenarUsuariosPorApellido(listaUsuarios);
        }
        public List<Usuario> FiltrarPorEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return _contexto.Usuarios.ToList();
            return _contexto.Usuarios.Where(u => !u.FechaBorrado.HasValue).ToList();
        }
        public int CrearUsuario(Usuario usuario)
        {
            usuario.FechaCreacion = DateTime.Now;
            usuario.Password = this.HashSHA256(usuario.Password);
            _contexto.Usuarios.Add(usuario);
            int ingreso = _contexto.SaveChanges();
            return ingreso;
        }

        public void EliminarUsuario(int id)
        {
            Usuario usuarioEncontrado = _contexto.Usuarios.Find(id);
            if (usuarioEncontrado != null)
            {
                usuarioEncontrado.FechaBorrado = DateTime.Now;
                usuarioEncontrado.FechaModificacion = usuarioEncontrado.FechaBorrado;
                _contexto.SaveChanges();
            }
        }
        public string HashSHA256(string valor)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(valor));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        public List<Usuario> OrdenarUsuariosPorApellido(List<Usuario> lista)
        {
            return lista.OrderBy(u => u.Apellido).ToList();
        }
        public void CerrarSesion()
        {
            AuthenticationHelper.SignOut(_httpContextAccessor.HttpContext);
        }
    }
}
