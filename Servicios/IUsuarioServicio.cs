using Entidades;
using Entidades.Entidades;
using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IUsuarioServicio
    {
        Usuario ValidarUsuario(string email, string password);
        List<Usuario> ListarUsuarios();
        List<Usuario> ListarUsuariosConEliminados();
        int CrearUsuario(Usuario usuario);
        Usuario EditarUsuario(Usuario usuario);
        Usuario BuscarUsuario(int id);
        void EliminarUsuario(int id);
        UsuarioResponse IniciarSesion(string email, string password);
        UsuarioModel IniciarSesionAPI(string email, string password);
        void CerrarSesion();
    }
}
