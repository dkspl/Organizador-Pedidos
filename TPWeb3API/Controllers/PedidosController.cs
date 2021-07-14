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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private IPedidoServicio PedidoServicio;
        private IArticuloServicio ArticuloServicio;
        private IClienteServicio ClienteServicio;

        public PedidosController(_20211CTPContext contexto)
        {
            PedidoServicio = new PedidoServicio(contexto);
            ArticuloServicio = new ArticuloServicio(contexto);
            ClienteServicio = new ClienteServicio(contexto);
        }

        [Route("Buscar")]
        [HttpPost]
        public string Buscar(int? IdCliente, int? IdEstado, string incluir)
        {
            List<PedidoModel> listaPedidos = PedidoServicio.ListarPedidoModels(IdCliente, IdEstado, incluir);

            string count = listaPedidos.Count().ToString();
            var returnObj = new { Count = count, Items = listaPedidos };
            string listaSerializada = JsonConvert.SerializeObject(returnObj, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return listaSerializada;
        }
    }
}
