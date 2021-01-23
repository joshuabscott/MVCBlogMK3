using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MVCBlogMK3.Data;

namespace MVCBlogMK3.Utilities
{
    public class DataUtility
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            // The default connection string will come from appsettings.json like usual
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // It will be automatically overwritten if we are running on Heroku
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        public static string BuildConnectionString(string databaseUrl)
        {
            // Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };
            return builder.ToString();
        }

        public static async Task ManageData(IHost host)
        {
            try
            {
                //This technique is used to obtain references to services
                // normally I would just inject these services but you cant use a constructor in a static class
                using var svcScope = host.Services.CreateScope();
                var svcProvider = svcScope.ServiceProvider;

                //The service will run your migrations
                var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
                await dbContextSvc.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while running Manage Data => {ex}");
            }
        }
    }
}

//Step 3: Modify how the connection string is built
//It is very common to have mechanisms in place for building a connection string differently based on the environment your application is running in. 
//In your case, you need to contend with a database running on localhost:5432 during development versus one running remotely in Heroku on some unknown port that is determined by Heroku at runtime. 
//An additional complexity requiring a programmatic solution is that Heroku periodically rotates their database credentials preventing us from hard-coding them in our settings.

//Notice how both the Uri and NpgsqlConnectionStringBuilder classes work in concert to provide an elegant approach 
//to deriving the connection string from the DATABASE_URL configuration variable attached to this Heroku application.


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