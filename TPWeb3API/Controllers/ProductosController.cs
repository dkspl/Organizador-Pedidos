using Entidades.Entidades;
using Entidades.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPWeb3API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private IArticuloServicio ArticuloServicio;

        public ProductosController(_20211CTPContext contexto)
        {
            ArticuloServicio = new ArticuloServicio(contexto);
        }
        [HttpGet]
        public string Get()
        {
            List<ArticuloModel> listaArticulo = ArticuloServicio.ListarArticuloModels(ArticuloServicio.ListarArticulos());

            string count = listaArticulo.Count().ToString();
            var returnObj = new { Count = count, Items = listaArticulo };
            string devolver = JsonConvert.SerializeObject(returnObj, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return devolver;
        }

        [Route("Filtrar")]
        [HttpPost]
        public string Filtrar(string filtro)
        {
            List<ArticuloModel> listaArticulos = ArticuloServicio.FiltrarArticulosPorNombre(filtro);

            string count = listaArticulos.Count().ToString();
            var returnObj = new { Count = count, Items = listaArticulos };
            string listaFiltrada = JsonConvert.SerializeObject(returnObj, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return listaFiltrada;
        }
    }
}
