using System;
using ASPNETCoreIdentity.Areas.Identity.Data;
using ASPNETCoreIdentity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ASPNETCoreIdentity.Areas.Identity.IdentityHostingStartup))]
namespace ASPNETCoreIdentity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ASPNETCoreIdentityContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("ASPNETCoreIdentityContextConnection")));

                services.AddDefaultIdentity<ASPNETCoreIdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ASPNETCoreIdentityContext>();
            });
        }
    }
}