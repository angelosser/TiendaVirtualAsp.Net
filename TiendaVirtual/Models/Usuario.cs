using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace TiendaVirtual.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Ingrese un nombre, por favor revisar")]
        [DataType(DataType.Text)]
        public string Nombres { get; set; }
        [DataType(DataType.Text)]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "Ingrese el DNI del usuario")]
        [MaxLength(8,ErrorMessage ="Solo se aceptan 8 caracteres")]
        [RegularExpression(@"^[0-9]+([\,\.][0-9]+)?$", ErrorMessage ="Formato no valido")]
        public string Dni { get; set; }
        [Required(ErrorMessage ="se requiere correo")]
        [DataType(DataType.EmailAddress,ErrorMessage ="formato de correo invalido")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Ingrese el DNI del usuario")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
        [Required(ErrorMessage ="Ingrese información")]
        public bool? Activo { get; set; }
    }
}
