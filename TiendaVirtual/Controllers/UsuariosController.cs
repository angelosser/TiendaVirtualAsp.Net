using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TiendaVirtual.Models; //para trabajar con el model de la base de datos
using Microsoft.EntityFrameworkCore; // para trabajar con funciones de EntityFramework
using Microsoft.AspNetCore.Authorization;

namespace TiendaVirtual.Controllers
{
    /// <summary>
    /// Este es el ejemplo de creación de un cotrolador de forma manual
    /// para crear las vistas se debe dar clic derecho en cada uno de los metodos 
    /// y seleccionar agregar vista, luego se debe indicar la platilla y modelo adecuados
    /// para el metodo
    /// </summary>
    /// 
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly TiendaVirtual20221Context bd;

        public UsuariosController(TiendaVirtual20221Context context)
        {
            bd = context;
        }
        public IActionResult Index()
        {
            return View(bd.Usuarios.ToList());
        }

        public IActionResult Details(int Id)
        {
            //metodo abrebviado
            //return View(bd.Usuarios.FirstOrDefault(x => x.Id == Id));

            //metodo mas completo con validacion
            var usuario = bd.Usuarios.FirstOrDefault(m => m.Id == Id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);

        }

      
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                bd.Add(usuario);
                bd.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var usuario = bd.Usuarios.FirstOrDefault(x => x.Id == Id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bd.Update(usuario);
                    bd.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var usuario = bd.Usuarios.FirstOrDefault(x => x.Id == Id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Delete(Usuario usuario)
        {
            var usr = bd.Usuarios.FirstOrDefault(x => x.Id == usuario.Id);
            bd.Usuarios.Remove(usr);
            bd.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        private bool UsuarioExists(int Id)
        {
            return bd.Usuarios.Any(e => e.Id == Id);
        }

    }
}
