using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVCBlogMK3.Models;
using MVCBlogMK3.Utilities;

namespace MVCBlogMK3
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await SeedDataAsync(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //Seed Data Method
        public async static Task SeedDataAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var userManager = services.GetRequiredService<UserManager<BlogUser>>();
                var roleManger = services.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedUtility.SeedDataAsync(userManager, roleManger);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
