//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Intex2.Models
//{
//    public static class IdentitySeedData
//    {
//        private const string adminUser = "Admin";
//        private const string adminPassword = "Superuser1!";

//        public static async void EnsurePopulated(IApplicationBuilder app)
//        {
//            AppIdentityDbContext context = app.ApplicationServices
//                .CreateScope().ServiceProvider
//                .GetRequiredService<AppIdentityDbContext>();

//            if (context.Database.GetPendingMigrations().Any())
//            {
//                context.Database.Migrate();
//            }

//            UserManager<IdentityUser> userManager = app.ApplicationServices
//                .CreateScope().ServiceProvider
//                .GetRequiredService<UserManager<IdentityUser>>();

//            IdentityUser newUser = await userManager.FindByIdAsync(adminUser);

//            if (newUser == null)
//            {
//                newUser = new IdentityUser(adminUser);

//                newUser.Email = "admin@yeet.com";
//                newUser.PhoneNumber = "555-1234";

//                await userManager.CreateAsync(newUser, adminPassword);
//            }
//        }
//    }
//}