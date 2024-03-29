﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Entidades.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TPWeb3.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ArticuloController : Controller
    {
        private IArticuloServicio ArticuloServicio;
        private IPedidoServicio PedidoServicio;
        private readonly INotyfService _notyf;

        public ArticuloController(_20211CTPContext contexto, INotyfService notyf)
        {
            ArticuloServicio = new ArticuloServicio(contexto);
            PedidoServicio = new PedidoServicio(contexto);
            _notyf = notyf;
        }
        public IActionResult Index(string incluir)
        {
            if (TempData["notificacion"] != null)
            {
                _notyf.Success("El artículo " + TempData["notificacion"] + " se ha creado con éxito.");
            }
            ViewBag.Incluir = incluir;
            if (!string.IsNullOrEmpty(incluir) && incluir.Equals("on"))
            {
                return View(ArticuloServicio.ListarArticulosConEliminados());
            }
            return View(ArticuloServicio.ListarArticulos());
        }
        public IActionResult NuevoArticulo()
        {
            if (TempData["notificacion"] != null)
            {
                _notyf.Success("El artículo " + TempData["notificacion"] + " se ha creado con éxito.");
            }
            return View();
        }
        [HttpPost]
        public IActionResult NuevoArticulo(Articulo articulo, int retorno)
        {
            if (ModelState.IsValid)
            {
                articulo.CreadoPor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
                if (ArticuloServicio.CrearArticulo(articulo) != null)
                {
                    TempData["notificacion"] = articulo.Codigo+"-"+articulo.Descripcion;
                    if (retorno == 0)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("NuevoArticulo");
                }
            }
            return View(articulo);
        }

        public IActionResult Detalle(int id)
        {
            Articulo articuloEncontrado = ArticuloServicio.BuscarArticulo(id);
            return View(articuloEncontrado);
        }
        [HttpPost]
        public IActionResult EliminarArticulo(int id)
        {
            int eliminadoPor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
            ArticuloServicio.EliminarArticulo(id, eliminadoPor);
            PedidoServicio.BorrarArticulosPorArticuloBorrado(id, eliminadoPor);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditarArticulo(Articulo articulo)
        {
            articulo.ModificadoPor = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
            ArticuloServicio.EditarArticulo(articulo);
            return Redirect("/Articulo/Detalle/" + articulo.IdArticulo);
        }
    }
}
