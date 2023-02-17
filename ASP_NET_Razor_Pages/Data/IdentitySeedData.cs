using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ASP_NET_Razor_Pages.Data
{
    public class IdentitySeedData
    {
        public static async Task Initialize(ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {

            context.Database.EnsureCreated();

            string adminRole = "Admin";
            string memberRole = "Member";
            string password4all = "zaq1@WSX";

            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            if (await roleManager.FindByNameAsync(memberRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(memberRole));
            }

            if (await userManager.FindByNameAsync("normalUser@test.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "normalUser@test.com",
                    Email = "normalUser@test.com",
                    PhoneNumber = "123456789"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password4all);
                    await userManager.AddToRoleAsync(user, memberRole);
                }
            }

            if (await userManager.FindByNameAsync("adminUser@test.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "adminUser@test.com",
                    Email = "adminUser@test.com",
                    PhoneNumber = "123456789"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password4all);
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }
        }
    }
}