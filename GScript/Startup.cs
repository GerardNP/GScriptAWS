using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GScript.Helpers;
using GScript.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using GScript.Data;
using Microsoft.EntityFrameworkCore;
using GScript.Repositories;

namespace GScript
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // TEMPDATA
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSession();
            //services.AddDistributedMemoryCache();

            //services.AddSession( options => {
            //    options.IdleTimeout = TimeSpan.FromMinutes(15);
            //});

            // INYECCIONES
            String mysqlconection = this.Configuration.GetConnectionString("awsmysql");
            services.AddDbContextPool<ScriptsContext>(
                options => options.UseMySql(mysqlconection, ServerVersion.AutoDetect(mysqlconection)));
            services.AddTransient<IRepositoryScripts, RepositoryScriptsSQL>();

            services.AddTransient<PathProvider>();

            String urlapiscripts = this.Configuration["urlapiscripts"];
            services.AddTransient(x => new ServiceScripts(urlapiscripts));


            // AUTHENTICATION
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();

            services.AddControllersWithViews(
                options => options.EnableEndpointRouting = false)
                .AddSessionStateTempDataProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes => 
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
