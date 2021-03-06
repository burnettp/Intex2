using Intex2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using System.Net;

namespace Intex2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            // Enable HSTS
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(.01);
            });

            //Enable HTTPS Redirection
            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
            //    options.HttpsPort = 443;
            //});

            services.AddControllersWithViews();

            // Connects MySQL database as the backend DB - hosted on AWS
            services.AddDbContext<CrashContext>(options =>
            {
                // Environment variable handles connection string for #security
                options.UseMySql(Environment.GetEnvironmentVariable("DbConnectionEnv"));
            });
            
            services.AddSingleton<InferenceSession>(

                new InferenceSession("wwwroot/intex2.onnx")

            );
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
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute("countypage",
                    "{county}/Page{pageNum}",
                    new { Controller = "Home", action = "Summary" });

                endpoints.MapControllerRoute("Paging", 
                    "Page{pageNum}", 
                    new { controller = "Home", action = "Summary", pageNum = 1 });

                endpoints.MapControllerRoute("county",
                    "{county}",
                    new { Controller = "Home", action = "Summary", pageNum = 1 });

                endpoints.MapControllerRoute("SearchResults",
                    "SearchResults/{crashid}",
                    new { Controller = "Home", action = "SearchResults"});

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "inference",

                    defaults: new { controller = "Inference", action = "Score" },

                    pattern: ""
                    );
                endpoints.MapRazorPages();
            });
        }
    }
}
