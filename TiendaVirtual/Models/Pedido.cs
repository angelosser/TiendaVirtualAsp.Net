using System;
using System.Collections.Generic;

#nullable disable

namespace TiendaVirtual.Models
{
    public partial class Pedido
    {
        public Pedido()
        {
            PedidoDetalles = new HashSet<PedidoDetalle>();
        }

        public int Id { get; set; }
        public int? IdCliente { get; set; }
        public int? IdTarjeta { get; set; }
        public DateTime? FechaHora { get; set; }
        public string Estado { get; set; }
        public decimal? Total { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Tarjetum IdTarjetaNavigation { get; set; }
        public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; }
    }
}
