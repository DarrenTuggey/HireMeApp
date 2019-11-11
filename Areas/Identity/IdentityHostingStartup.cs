using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HireMeApp.Areas.Identity;
using HireMeApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace HireMeApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        private static string Secrets()
        {
            string kvUri = "https://HireMeVault.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret("ConnectStr");
            string key = secret.Value;
            return key;
        }

        private readonly string _conStr = Secrets();

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                //var connBuilder = new SqlConnectionStringBuilder(
                //    context.Configuration.GetConnectionString("HireMeAppAuthConnection"));
                //connBuilder.UserID = context.Configuration["DbUsername"];
                //connBuilder.Password = context.Configuration["DbPassword"];

                services.AddDbContext<HireMeAppAuth>(options =>
                    options.UseSqlServer(_conStr));//connBuilder.ConnectionString));

                services.AddDefaultIdentity<HireMeAppUser>()
                    .AddDefaultUI()//(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<HireMeAppAuth>();
            });
        }
    }
}