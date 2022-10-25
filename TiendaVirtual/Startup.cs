using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//referencias para usar el El contexto de base de datos
using TiendaVirtual.Models;
using Microsoft.EntityFrameworkCore;
//referencias para poder usar Authentication
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace TiendaVirtual
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //Se registra el contexto de la base de datos (modelo) como servicio en el proyecto
            services.AddDbContext<TiendaVirtual20221Context>(options => options.UseSqlServer(Configuration.GetConnectionString("TiendaVirtualDBConn")));

            //Se registra el modo de Authenticación como servicio en el proyecto
            //https://stackoverflow.com/questions/44018218/asp-net-core-simplest-possible-forms-authentication/44018596
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o => o.LoginPath = new PathString("/Account/login"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            
            

            app.UseRouting();

            // Esta linea debe ser agregada antes de definir los UseEndPoints, 
            // Con esta línea de codigo activamos el uso de authentication en el proyecto
            // ¿Quien eres tu para esta app?
            app.UseAuthentication();
            // ¿Que tienes permitido hacer dentro de la app?
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
