using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaVirtualWebAPI.ViewModel
{
    public class vmProducto
    {
        public int Id { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdMarca { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string Url { get; set; }
        public bool? Destacado { get; set; }
        public bool? Activo { get; set; }
    }
}
