using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MVCBlogMK3.Models;
using MVCBlogMK3.Data;
using MVCBlogMK3.Enums;

namespace MVCBlogMK3.Utilities
{
    public static class SeedUtility
    {
        public static async Task SeedDataAsync(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);          //calls 3 private methods
            await SeedAdministrator(userManager);   //calls 3 private methods
            await SeedModerator(userManager);     //calls 3 private methods
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));    //adds 2 Toles, setup as Enums
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));    //adds 2 Toles, setup as Enums
        }

        public static async Task SeedAdministrator(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("J@mailinator.com") == null)     //adds me as BlogUser - Admin
            {
                var admin = new BlogUser()
                {
                    UserName = "J@mailinator.com",
                    Email = "J@mailinator.com",
                    FirstName = "Josh",
                    LastName = "Scott",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "!1Qwerty");
                await userManager.AddToRoleAsync(admin, Roles.Administrator.ToString());
            }
        }

        public static async Task SeedModerator(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("W@mailinator.com") == null)     //adds a fake BlogUser - Moderator
            {
                var moderator = new BlogUser()
                {
                    UserName = "W@mailinator.com",
                    Email = "W@mailinator.comJ",
                    FirstName = "Adam",
                    LastName = "West",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(moderator, "!1Qwerty");
                await userManager.AddToRoleAsync(moderator, Roles.Moderator.ToString());
            }
        }
    }
}
