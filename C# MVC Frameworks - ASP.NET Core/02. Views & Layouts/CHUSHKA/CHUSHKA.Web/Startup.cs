using System;
using CHUSHKA.Data;
using CHUSHKA.Models;
using CHUSHKA.Web.Services;
using CHUSHKA.Web.Services.Contracts;
using CHUSHKA.Web.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CHUSHKA.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ChushkaDbContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 3;
            });

            services.AddIdentity<User, IdentityRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ChushkaDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IProductService, ProductService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            RolesSeeder.Seed(serviceProvider);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
