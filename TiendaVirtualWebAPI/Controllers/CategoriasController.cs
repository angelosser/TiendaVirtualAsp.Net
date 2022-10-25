using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualWebAPI.Models;
using TiendaVirtualWebAPI.ViewModel;

namespace TiendaVirtualWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly TiendaVirtual20221Context _context;

        public CategoriasController(TiendaVirtual20221Context context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public ActionResult<IEnumerable<vmCategoria>> GetCategoria()
        {
            List<vmCategoria> lst = new List<vmCategoria>();

            var _Categorias = (from Categorium c in _context.Categoria
                               where c.Activo == true
                               select c).ToList();

            foreach(var item in _Categorias )
            {
                lst.Add(new vmCategoria() { Id = item.Id, Activo = item.Activo, 
                    Nombre = item.Nombre }
                        );

            }
            return Ok(lst);
        }


        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vmCategoria>> GetCategorium(int id)
        {
            var categorium = await _context.Categoria.FindAsync(id);

            if (categorium != null)
            {
                vmCategoria _Categoria = new vmCategoria() { Id = categorium.Id,Activo=categorium.Activo,
                                                                Nombre=categorium.Nombre
                                                            };
                return Ok(_Categoria);
            }

            return NotFound();

        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutCategorium([FromBody] vmCategoria pCat)
        {

            if (pCat==null) return BadRequest();
            //Buscando el registro de categoria
            Categorium Catbd = _context.Categoria.FirstOrDefault(x => x.Id == pCat.Id);

            if (Catbd == null) //no existe
            {
                return NotFound();
            }

                //Actualizando campos
                Catbd.Nombre = pCat.Nombre;
                Catbd.Activo = pCat.Activo;
                //Indicando al modelo que el registro tiene modificaciones
                _context.Entry(Catbd).State = EntityState.Modified;
                await _context.SaveChangesAsync(); //grabando cambios
                return Ok(pCat);
           
        }

        // POST: api/Categorias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<vmCategoria>> PostCategorium([FromBody]vmCategoria pCat)
        {
            Categorium Catbd = new Categorium() { Activo = pCat.Activo, Nombre = pCat.Nombre };
            if(ModelState.IsValid)
            { 
                _context.Categoria.Add(Catbd);
                await _context.SaveChangesAsync();
            
                pCat.Id = Catbd.Id;

                return Ok(pCat);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categorium>> DeleteCategorium(int id)
        {
            var categorium = await _context.Categoria.FindAsync(id);
            if (categorium == null)
            {
                return NotFound();
            }

            _context.Categoria.Remove(categorium);
            await _context.SaveChangesAsync();

            return categorium;
        }

        private bool CategoriumExists(int id)
        {
            return _context.Categoria.Any(e => e.Id == id);
        }
    }
}
