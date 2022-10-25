﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaVirtualWebAPI.ViewModel
{
    public class vmPedidoDetalle
    {

        public int Id { get; set; }
        public int? IdPedido { get; set; }
        public int? IdProducto { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? SubTotal { get; set; }
        
        public vmProducto Producto { get; set; }
    }
}
