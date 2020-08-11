using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SalesWebMvc.Services;

namespace SalesWebMvc
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                                
                                //tipo do DbContext criado na pasta DATA
            services.AddDbContext<SalesWebMvcContext>(options =>       // nome da classe de context
                    options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), 
                    builder => builder.MigrationsAssembly("SalesWebMvc")));
            // nome do assembly (projeto)

            // instalar o provider do MySql para usar o UseMySql()
            // abrir o terminal do nuget (gerenciador de pacotes do .net)
            // tools > nuget Package manager > package manager console
            // OBS.: instalar a versão do provider de acordo com a versão do .net em uso
            // no caso: Install-Package Pomelo.EntityFrameworkCore.MySql -Version 2.1.1


            //serviço para popular banco de dados
            services.AddScoped<SeedingService>(); // registra o serviço no sistema de injeção de dependencia da aplicacao
            
            // serviços de entidades de negócio
            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
                                                                                  // colocando o servico registrado no sistema de injeção de dependencia e ele já cria um objeto instanciado
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seedingService.Seed(); // só chamar o método 
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
