using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaVirtualWebAPI.ViewModel;
using TiendaVirtualWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TiendaVirtualWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly TiendaVirtual20221Context _context;

        public ProductosController(TiendaVirtual20221Context context)
        {
            _context = context;
        }
        // GET: api/<ProductosController>
        [HttpGet]
        public ActionResult<IEnumerable<vmProducto>> Get()
        {
            var lst = (from x in _context.Productos
                       where x.Activo == true
                       select new vmProducto
                       {
                           Activo = x.Activo,
                           Descripcion = x.Descripcion,
                           Destacado = x.Destacado,
                           Id = x.Id,
                           IdCategoria = x.IdCategoria,
                           IdMarca = x.IdMarca,
                           Nombre = x.Nombre,
                           Precio = x.Precio,
                           Url = x.Url
                       }
                    ).ToList();


            return Ok(lst);
        }
        // GET api/<ProductosController>/PorCategoria?IdCategoria=1
        [Route("PorCategoria")]
        [HttpGet]
        public ActionResult<IEnumerable<vmProducto>> GetPorCategoria(int IdCategoria)
        {
            var lst = (from x in _context.Productos
                       where x.Activo == true && x.IdCategoria == IdCategoria
                       select new vmProducto
                       {
                           Activo = x.Activo,
                           Descripcion = x.Descripcion,
                           Destacado = x.Destacado,
                           Id = x.Id,
                           IdCategoria = x.IdCategoria,
                           IdMarca = x.IdMarca,
                           Nombre = x.Nombre,
                           Precio = x.Precio,
                           Url = x.Url
                       }
                    ).ToList();


            return Ok(lst);
        }

        // GET api/<ProductosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vmProducto>> Get(int id)
        {
            var bdprod = await _context.Productos.FindAsync(id);

            if (bdprod != null)
            {
                vmProducto objProd = new vmProducto()
                {
                    Activo = bdprod.Activo,
                    Descripcion = bdprod.Descripcion,
                    Destacado = bdprod.Destacado,
                    Id = bdprod.Id,
                    IdCategoria = bdprod.IdCategoria,
                    IdMarca = bdprod.IdMarca,
                    Nombre = bdprod.Nombre,
                    Precio = bdprod.Precio,
                    Url = bdprod.Url
                };
                return Ok(objProd);
            }

            return NotFound();

        }

        // POST api/<ProductosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
