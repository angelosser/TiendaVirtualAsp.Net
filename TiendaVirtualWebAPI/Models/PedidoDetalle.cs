using System;
using System.Collections.Generic;

#nullable disable

namespace TiendaVirtualWebAPI.Models
{
    public partial class PedidoDetalle
    {
        public int Id { get; set; }
        public int? IdPedido { get; set; }
        public int? IdProducto { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? SubTotal { get; set; }

        public virtual Pedido IdPedidoNavigation { get; set; }
        public virtual Producto IdProductoNavigation { get; set; }
    }
}
