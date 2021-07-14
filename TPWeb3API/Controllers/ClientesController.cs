using Entidades.Entidades;
using Entidades.Models;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private IClienteServicio ClienteServicio;

        public ClientesController(_20211CTPContext contexto)
        {
            ClienteServicio = new ClienteServicio(contexto);
        }
        [HttpGet]
        public string Get()
        {
            List<ClienteModel> listaClientes = ClienteServicio.ListarClienteModels(ClienteServicio.ListarClientes());

            string count = listaClientes.Count().ToString();
            var returnObj = new { Count = count, Items = listaClientes };
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
            List<ClienteModel> listaClientes = ClienteServicio.FiltrarClientesPorNombre(filtro);

            string count = listaClientes.Count().ToString();
            var returnObj = new { Count = count, Items = listaClientes };
            string listaFiltrada = JsonConvert.SerializeObject(returnObj, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return listaFiltrada;
        }
    }
}
