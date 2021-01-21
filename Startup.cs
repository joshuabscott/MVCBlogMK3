using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVCBlogMK3.Data;
using MVCBlogMK3.Models;
using MVCBlogMK3.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MVCBlogMK3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //1. services are configured for using DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //2. using directive for injection using IdentityRole with BlogUser
            services.AddIdentity</*IdentityUser*/BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false) //Step 2 Add BlogUser, Identity Role
                .AddEntityFrameworkStores<ApplicationDbContext>()                                                     //-----true = scaffolded to start, false = changed to for building?,
                .AddDefaultUI()//Step 2 Add
                .AddDefaultTokenProviders();//Step 2 Add

            //3. instance of SeedUtility to be available as an injected dependency
            //services.AddTransient<SeedUtility>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.----------------------------------------------
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager)//Step 2 Add
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
