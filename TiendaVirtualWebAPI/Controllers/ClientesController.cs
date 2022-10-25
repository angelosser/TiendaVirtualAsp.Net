using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaVirtualWebAPI.Models;
using TiendaVirtualWebAPI.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TiendaVirtualWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly TiendaVirtual20221Context _context;

        public ClientesController(TiendaVirtual20221Context context)
        {
            _context = context;
        }
        // GET: api/<ClientesController>
        [HttpGet]
        public ActionResult<List<vmCliente>> Get()
        {
            List<vmCliente> lst = new List<vmCliente>();
            var cats = (from Cliente c in _context.Clientes
                        select c).ToList();
            foreach (var item in cats)
            {
                lst.Add(new vmCliente()
                {
                    Apellidos = item.Apellidos,
                    Correo = item.Correo,
                    Direccion = item.Direccion,
                    Dni = item.Dni,
                    Id = item.Id,
                    Nombres = item.Nombres,
                    Telefono = item.Telefono
                }
                    );

            }

            return Ok(lst);
        }
        [HttpPost("autenticar")]
        public IActionResult Autenticar([FromBody] vmCliente pcliente)
        {
            var cat =  _context.Clientes.FirstOrDefault(x => x.Dni == pcliente.Dni && x.Correo == pcliente.Correo);

            if (cat != null)
            {
                vmCliente obj = new vmCliente()
                {
                    Apellidos = cat.Apellidos,
                    Correo = cat.Correo,
                    Direccion = cat.Direccion,
                    Dni = cat.Dni,
                    Id = cat.Id,
                    Nombres = cat.Nombres,
                    Telefono = cat.Telefono
                };
                return Ok(obj);
            }

            return NotFound();


        }
        // GET api/<ClientesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClientesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ClientesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
