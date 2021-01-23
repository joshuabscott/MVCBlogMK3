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
            await DataUtility.ManageData(host);
            await SeedDataAsync(host);
            host.Run();
            //Step 6: Add programmatic migration - DataUtility
            //Updating the Main method to run asynchronously allows you to put the final piece of code in place in terms of database migrations. 
            //Calling the ManageData method in Main is actually responsible for the migrations being applied programmatically every time the application runs.
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //Step 6: Configure error handling
                    //I want to see as much error-related information as possible both during development as well as after a deployment to Heroku. 
                    //IWebHostBuilder gives you a certain level of control when it comes to capturing these errors.

                    //Setting CaptureStartupErrors to true forces the host to capture exceptions during startup and attempts to start the server. 
                    //Enabling DetailedErrors results in the application capturing detailed error information.

                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
                    webBuilder.UseStartup<Startup>();

                    //Step 6: Add programmatic migration
                    //There are numerous techniques for running code-first migrations and thus far you have only seen the approach that uses a command-line instruction 
                    //to create a named migration and another to apply the migration against a local database.

                    //PM > Add - Migration Initial
                    //PM > Update - Database

                    //The same approach could also be used to create migrations and apply them to a remote database.
                    //The Update-Database command simply looks for a valid connection string and if it has been updated to connect to the remote Heroku database it will work as expected.
                    //Although there is nothing wrong with this approach you will be doing it differently and will use a programmatic approach to applying your migrations.

                    //Once you have a reference to the ApplicationDbContext service you can call its MigrateAsync() method to programmatically apply migrations to the attached database. 
                    //This approach works for both local and remote instances and should be called from the Main method.
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
