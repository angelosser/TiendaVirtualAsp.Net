using System;
using System.Collections.Generic;

#nullable disable

namespace TiendaVirtualWebAPI.Models
{
    public partial class Tarjetum
    {
        public Tarjetum()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int Id { get; set; }
        public string Marca { get; set; }
        public string Numero { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
