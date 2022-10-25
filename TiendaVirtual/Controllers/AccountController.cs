using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TiendaVirtual.Models;
using TiendaVirtual.ViewModel;
//Referencias para usar authentication
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace TiendaVirtual.Controllers
{
    public class AccountController : Controller
    {
        private readonly TiendaVirtual20221Context bd;

        public AccountController(TiendaVirtual20221Context context)
        {
            bd = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            ViewData["mensaje"] = "";
            return View();

        }

        [HttpPost]
        public IActionResult Login(UsuarioLogin usuario)
        {
            ViewData["mensaje"] = "";
            if (!string.IsNullOrEmpty(usuario.Correo) && string.IsNullOrEmpty(usuario.Contrasena))
            {
                ViewData["mensaje"] = "no ingreso correo y/o contraseña";
                //Si lo enviado es nulo o vacio
                return RedirectToAction("Login");
            }

            //Aqui debemos verificar el usuario y passwor
            //Podemos llamar a la basae de dato o un servicio
            //https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-core-mvc-using-cookie/
            //https://stackoverflow.com/questions/44018218/asp-net-core-simplest-possible-forms-authentication/44018596
            //https://www.youtube.com/watch?v=Fhfvbl_KbWo&list=PLOeFnOV9YBa7dnrjpOG6lMpcyd7Wn7E8V


            //if(ModelState.IsValid)
            //{ 
                if (bd.Usuarios.Count(x => (x.Contrasena == usuario.Contrasena && x.Correo == usuario.Correo))>0)
                {

                    //Creamos el objeto identity para el usuario
                    // objeto identity estara disponible en todo el proyecto para validar los accesos
                    var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, usuario.Correo)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);


                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);



                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["mensaje"] = "error correo y/o contraseña no válidas";
                }
           // }
            return View();
        }

        //[HttpPost]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Account");
        }
    }
}
