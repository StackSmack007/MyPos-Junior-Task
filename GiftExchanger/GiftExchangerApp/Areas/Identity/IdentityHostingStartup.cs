using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(GiftExchangerApp.Areas.Identity.IdentityHostingStartup))]
namespace GiftExchangerApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}