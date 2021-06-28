﻿using Entidades.Entidades;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPWeb3.Controllers
{
    public class UsuarioController : Controller
    {
        private IUsuarioServicio _ServicioUsuario;

        public UsuarioController(_20211CTPContext context)
        {
            _ServicioUsuario = new UsuarioServicio(context);
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
        public IActionResult NuevoUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (_ServicioUsuario.CrearUsuario(usuario) == 1)
                    return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuario)
        {
            _ServicioUsuario.EditarUsuario(usuario);
            return Redirect("/Usuario/Detalle/" + usuario.IdUsuario);
        }
        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            _ServicioUsuario.EliminarUsuario(id);
            return RedirectToAction("Index");
        }
    }
}