﻿using Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IPedidoServicio
    {
        List<Pedido> ListarPedidosActivos();
        List<Pedido> ListarPedidosConEliminados();
        int CrearPedido(Pedido pedido, Cliente cliente);
        Pedido BuscarPedido(int id);
        List<EstadoPedido> listarEstadosPedido();
        void EditarPedido(Pedido pedido);
        List<PedidoArticulo> BuscarArticulosDeUnPedido(int id);
        void EditarArticulosDeUnPedido(Pedido pedido);
        List<Pedido> ListarPedidosAbiertos();
        List<Pedido> ListarPedidosCerrados();
    }
}