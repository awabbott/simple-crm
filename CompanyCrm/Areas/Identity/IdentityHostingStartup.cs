using System;
using CompanyCrm.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CompanyCrm.Areas.Identity.IdentityHostingStartup))]
namespace CompanyCrm.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CompanyCrmContext>(options =>
                    options.UseSqlServer(
                        context.Configuration
                        .GetConnectionString("CompanyCrmContextConnection")));

                services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                    options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<CompanyCrmContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();

                services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
                services.AddTransient<IEmailSender, EmailSender>();

                services.AddAuthentication()
                    .AddGoogle(o =>
                    {
                        o.ClientId = "266695265052-72rbtoq530qe84as86cq4hvi9cetp3j6.apps.googleusercontent.com";
                        o.ClientSecret = context.Configuration["Google:ClientSecret"];
                    });
            });
        }
    }
}