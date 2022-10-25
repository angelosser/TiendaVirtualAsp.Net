using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaVirtual.ViewModel
{
    public class UsuarioLogin
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Ingrese email")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "formato de correo invalido")]
        public string Correo { get; set; }
        //[Required(ErrorMessage = "Ingrese contraseña")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
    }
}
