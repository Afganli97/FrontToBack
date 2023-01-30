using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.Services;
using FrontToBack.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddControllersWithViews();
            services.AddDbContext<DataBase>(option => {
                option.UseSqlite(_config["ConnectionStrings:DefaultConnection"]);
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IBasketCount, BasketCountService>();

            services.AddIdentity<AppUser, IdentityRole>(options => {
                options.User.RequireUniqueEmail = true;

                //Yoxlama merhelesi rahat olsun deye komente atmisham
                //options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<DataBase>().AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            //app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "areas",
                    "{area:exists}/{controller=dashboard}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=home}/{action=index}/{id?}"
                    );
            });
        }
    }
}
