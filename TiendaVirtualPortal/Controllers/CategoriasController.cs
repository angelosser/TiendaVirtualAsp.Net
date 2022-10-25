using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


//Librerias para consumo de Web Services
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

//Librerias para el fomateo de los Objetos Json
using System.Text;
using Newtonsoft.Json;

using TiendaVirtualPortal.ViewModel;


namespace TiendaVirtualPortal.Controllers
{
    public class CategoriasController : Controller
    {
        public string BaseUrl { get; set; }

        public CategoriasController()
        {
            this.BaseUrl = "http://localhost:1123/api/Categorias";
        }


        // GET: CategoriasController
        public async Task<ActionResult> Index()
        {
            List<vmCategoria> lst = new List<vmCategoria>();

            HttpClient client = new HttpClient(); // declarando conector

            client.BaseAddress = new Uri(BaseUrl); // Estableciendo URL del servicio
            // Capturando Respuesta HTTP >>>>>      Método Http Get
            HttpResponseMessage resp = await client.GetAsync(""); //

            if(resp.IsSuccessStatusCode)
            { // si llamada es correcta
                //Obteniendo el resultado en cadena
                var readtask = resp.Content.ReadAsStringAsync().Result;
                //deserializa   de cadena a objeto
                lst = JsonConvert.DeserializeObject<List<vmCategoria>>(readtask);
            }

            return View(lst);
        }



        // GET: CategoriasController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            vmCategoria _Cat;
            HttpClient client = new HttpClient(); // declarando conector
            client.BaseAddress = new Uri(BaseUrl+"/"+id.ToString()); // Estableciendo URL del servicio
            // Capturando Respuesta HTTP >>>>>      Método Http Get
            HttpResponseMessage resp = await client.GetAsync("");
            if(resp.IsSuccessStatusCode)
            {
                //si la llamada fue exitosa
                var readtask = resp.Content.ReadAsStringAsync().Result;
                // deseralizar
                _Cat = JsonConvert.DeserializeObject<vmCategoria>(readtask);
                return View(_Cat);
            }

            return View();
        }

        // GET: CategoriasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(vmCategoria pCat)
        {
            try
            {
                vmCategoria CategoriaResultado = new vmCategoria();
                HttpClient client = new HttpClient(); // declarando conector
                client.BaseAddress = new Uri(BaseUrl);// Estableciendo URL del servicio
                // Serializar      de objeto a cadena
                StringContent content = new StringContent(JsonConvert.SerializeObject(pCat), Encoding.UTF8, "application/json");
                // Capturando Respuesta HTTP >>>>>      Método Http POST
                HttpResponseMessage resp =await client.PostAsync("", content);
                if(resp.IsSuccessStatusCode)
                {
                    var readtask = resp.Content.ReadAsStringAsync().Result;
                    //deserializa   de cadena a objeto
                    CategoriaResultado = JsonConvert.DeserializeObject<vmCategoria>(readtask);
                }


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriasController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            vmCategoria _Cat;
            HttpClient client = new HttpClient(); // declarando conector
            client.BaseAddress = new Uri(BaseUrl + "/" + id.ToString()); //se establece el url a llamar
            // invocamos verbo Get
            HttpResponseMessage resp = await client.GetAsync("");
            if (resp.IsSuccessStatusCode)
            {
                //si la llamada fue exitosa
                var readtask = resp.Content.ReadAsStringAsync().Result;
                // deseralizar
                _Cat = JsonConvert.DeserializeObject<vmCategoria>(readtask);
                return View(_Cat);
            }

            return View();
        }

        // POST: CategoriasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(vmCategoria pCat)
        {
            try
            {
                vmCategoria CategoriaResultado = new vmCategoria();
                HttpClient client = new HttpClient(); // declarando conector
                client.BaseAddress = new Uri(BaseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(pCat), Encoding.UTF8, "application/json");
                HttpResponseMessage resp = await client.PutAsync("", content);
                if (resp.IsSuccessStatusCode)
                {
                    var readtask = resp.Content.ReadAsStringAsync().Result;
                    CategoriaResultado = JsonConvert.DeserializeObject<vmCategoria>(readtask);
                }


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriasController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            vmCategoria _Cat;
            HttpClient client = new HttpClient(); // declarando conector
            client.BaseAddress = new Uri(BaseUrl + "/" + id.ToString()); //se establece el url a llamar
            // invocamos verbo Get
            HttpResponseMessage resp = await client.GetAsync("");
            if (resp.IsSuccessStatusCode)
            {
                //si la llamada fue exitosa
                var readtask = resp.Content.ReadAsStringAsync().Result;
                // deseralizar
                _Cat = JsonConvert.DeserializeObject<vmCategoria>(readtask);
                return View(_Cat);
            }

            return View();
        }

        // POST: CategoriasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                vmCategoria _Cat = new vmCategoria();
                
                HttpClient client = new HttpClient(); // declarando conector
                client.BaseAddress = new Uri(BaseUrl + "/" + id.ToString()); //se establece el url a llamar
                                                                             // invocamos verbo Get
                HttpResponseMessage resp = await client.DeleteAsync("");
                if (resp.IsSuccessStatusCode)
                {
                    //si la llamada fue exitosa
                    var readtask = resp.Content.ReadAsStringAsync().Result;
                    // deseralizar
                    _Cat = JsonConvert.DeserializeObject<vmCategoria>(readtask);
                    
                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
