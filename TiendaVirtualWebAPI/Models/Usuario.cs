using System;
using System.Collections.Generic;

#nullable disable

namespace TiendaVirtualWebAPI.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public bool? Activo { get; set; }
    }
}
