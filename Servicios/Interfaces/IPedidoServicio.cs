﻿using Entidades.Entidades;
using Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IPedidoServicio
    {
        Pedido CrearPedido(Pedido pedido, Cliente cliente);
        Pedido BuscarPedido(int id);
        List<EstadoPedido> ListarEstadosPedido();
        void EditarPedido(Pedido pedido);
        List<PedidoArticulo> BuscarArticulosDeUnPedido(int id);
        List<PedidoArticulo> EditarArticulosDeUnPedido(List<PedidoArticulo> listaArticulos, int idPedido);
        List<Pedido> ListarTodosLosPedidos();
        List<Pedido> ListarPedidosAbiertos(List<Pedido> listaPedidos);
        void EliminarPedido(int idPedido);
        void CerrarPedido(int idPedido);
        void EntregarPedido(int idPedido);
        List<Pedido> ListarPedidos(int? cliente, int? estado, string incluir);
        List<PedidoModel> ListarPedidoModels(int? cliente, int? estado, string incluir);
        List<ArticuloCantidadModel> ListarArticulosConCantidades(List<PedidoArticulo> articulos);
    }
}
