using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TiendaVirtualWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TiendaVirtualWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly TiendaVirtual20221Context bd;

        public UsuariosController(TiendaVirtual20221Context context)
        {
            bd = context;
        }
        [Route("Listar")]
        [HttpGet]
        public IEnumerable<Usuario> Listar()
        {
            return bd.Usuarios.ToList();
        }
        [Route("ListarActivos")]
        [HttpGet]
        public IEnumerable<Usuario> ListarActivos()
        {
            var lstUsuario = (from Usuario u in bd.Usuarios
                              where u.Activo == true
                              select u).ToList();
            return lstUsuario;

        }


        [HttpGet("{id}", Name = "BuscarUsuario")]
        public IActionResult ObtenerUsuario(int Id)
        {
            var objusuario = bd.Usuarios.FirstOrDefault(x => x.Id == Id);

            if (objusuario == null)
            {
                return NotFound();
            }

            return Ok(objusuario);

        }

       [Route("autenticar")]
        [HttpPost]
        public IActionResult Authenticar(string pCorreo, string pContrasena)
        {
            var objusuario = bd.Usuarios.FirstOrDefault(x => x.Correo == pCorreo && x.Contrasena== pContrasena);

            if (objusuario == null)
            {
                return NotFound();
            }

            return Ok(objusuario);

        }


        [Route("Crear")]
        
        public IActionResult CrearUsuario([FromBody] Usuario pUsr)
        {
            if (ModelState.IsValid)
            {
                bd.Add(pUsr);
                bd.SaveChanges();
                return Ok(pUsr);

            }

            return BadRequest(ModelState);

        }

        [Route("Editar")]
        [HttpPut]
        public IActionResult EditarUsuario([FromBody] Usuario pUsr)
        {
            

            // Verificando si existe
            Usuario UsrValido = bd.Usuarios.FirstOrDefault(x => x.Id == pUsr.Id);
            if (UsrValido == null)
            {
                return NotFound();
            }

            UsrValido.Activo = pUsr.Activo;
            UsrValido.Apellidos = pUsr.Apellidos;
            UsrValido.Contrasena = pUsr.Contrasena;
            UsrValido.Correo = pUsr.Correo;
            UsrValido.Dni = pUsr.Dni;
            UsrValido.Nombres = pUsr.Nombres;

            //bd.Entry(pUsr).State = EntityState.Modified;

            bd.SaveChanges();
            return Ok(pUsr);

        }

        //[Route("Eliminar")]
        [HttpDelete("{Id}")]
        public IActionResult EliminarUsuario (int Id)
        {
            // Verificando si existe
            Usuario UsrValido = bd.Usuarios.FirstOrDefault(x => x.Id == Id);
            if (UsrValido == null)
            {
                return NotFound(); //retorno no encontrado
            }
            bd.Usuarios.Remove(UsrValido);
            bd.SaveChanges();
            return Ok(UsrValido);

        }



    }
}
