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

            services.AddControllersWithViews();

            // Connects MySQL database as the backend DB - hosted on AWS
            services.AddDbContext<CrashContext>(options =>
            {
                options.UseMySql(Configuration["ConnectionStrings:DbConnection"]);
            });

            services.AddSingleton<InferenceSession>(
                new InferenceSession("C:/Users/parke/source/repos/Intex2/Intex2/Models/intex2.onnx")
            );

            // Google authenticator
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });
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
