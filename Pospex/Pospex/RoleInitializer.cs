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
            string adminName = "admin";
            string adminPassword = "Admin-1111";

            string userName = "user";
            string userPassword = "User-1111";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await userManager.FindByNameAsync(adminName) == null)
            {
                User admin = new User { Email = adminName, UserName = adminName, Avatar = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "./wwwroot/img/adminImg.jpg")) };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await userManager.FindByNameAsync(userName) == null)
            {
                User user = new User { Email = userName, UserName = userName, Avatar = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "./wwwroot/img/userImg.jpg")) };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
