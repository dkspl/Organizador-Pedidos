﻿using Entidades.Entidades;
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
    }
}
