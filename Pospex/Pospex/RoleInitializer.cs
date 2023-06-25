using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Pospex.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pospex
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin-mail@yandex.ru";
            string adminPassword = "Admin-1111";

            string userEmail = "user-mail@yandex.ru";
            string userPassword = "User-1111";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, Avatar = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "./wwwroot/img/adminImg.jpg")) };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                User user = new User { Email = userEmail, UserName = userEmail, Avatar = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "./wwwroot/img/userImg.jpg")) };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
