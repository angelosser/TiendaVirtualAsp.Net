using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TiendaVirtualWebAPI.ViewModel
{
    public class vmPedido
    {
        public vmPedido()
        {
            Detalles = new List<vmPedidoDetalle>();
        }

        public int Id { get; set; }
        public int? IdCliente { get; set; }
        public int? IdTarjeta { get; set; }
        public DateTime? FechaHora { get; set; }
        public string Estado { get; set; }
        public decimal? Total { get; set; }

        public vmCliente Cliente { get; set; }
        public vmTarjeta Tarjeta { get; set; }

        public List<vmPedidoDetalle> Detalles { get; set; }
    }
}
