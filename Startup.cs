using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HireMeApp.Data;
using HireMeApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace HireMeApp
{
    public class Startup
    {
        // This retrieves the connection string from Azure KeyVault
        private static string Secrets()
        {
            string kvUri = "https://HireMeVault.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret("ConnectStr");
            string key = secret.Value;
            return key;
        }

        private readonly string _conStr = Secrets();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Added LookupService for DI 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<HireMeContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString(_conStr)));
                options.UseSqlServer(_conStr));
            services.AddTransient<LookupService>();
           // services.AddSingleton<AdminRegistrationTokenService>();
            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            IConfigurationSection cpServicesConfig = Configuration.GetSection("HireMeAppServices");


            services.AddSingleton(new QRCodeService(new QRCodeGenerator()));
            services.AddSingleton<AdminRegistrationTokenService>();

            //services.AddAuthorization(options =>
            //    options.AddPolicy("Admin", policy =>
            //        policy.RequireAuthenticatedUser()
            //            .RequireClaim("IsAdmin", bool.TrueString)));

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                    options.Conventions.AuthorizePage("/Products/Edit", "Admin"));

            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            
            endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
            
            
            
            
            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
