using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using NodeManager.Domain;
using NodeManager.Web.Abstract;
using NodeManager.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace NodeManager.Web
{
    public class Startup
    {
        private IConfigurationRoot _configString;

        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnv)
        {
            _configString = new ConfigurationBuilder().SetBasePath(hostEnv.ContentRootPath).AddJsonFile("appsettings.json").Build();
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NodeManagerDBEntities>(options => options.UseSqlServer(_configString.GetConnectionString("NodeManagerDBEntities")));
            services.AddControllersWithViews();
            services.AddTransient<INodes, NodeRep>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Node2",
                    pattern: "Node/List",
                    defaults: new { controller = "Node", action = "List", category = (string)null, page = 1 });

                endpoints.MapControllerRoute(
                    name: "Node1",
                    pattern: "Node/List/{category}",
                    defaults: new { controller = "Node", action = "List", category = "category", page = 1 });

                endpoints.MapControllerRoute(
                    name: "Node",
                    pattern: "Node/List/{category}/{page}",
                    defaults: new { controller = "Node", action = "List", category = "category", page = "page" });

                endpoints.MapControllerRoute(
                    name: "FamSymbol",
                    pattern: "Node/FamSymbol/{id}",
                    defaults: new { controller = "Node", action = "FamSymbol", id="id" });

                endpoints.MapControllerRoute(
                   name: "Nav1",
                   pattern: "Nav/Menu",
                   defaults: new { controller = "Nav", action = "Menu", category = (string)null });

                endpoints.MapControllerRoute(
                   name: "Nav",
                   pattern: "Nav/Menu/{category}",
                   defaults: new { controller = "Nav", action = "Menu", category = "category" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Node}/{action=List}/{category?}");

                
            });
        }
    }
}
