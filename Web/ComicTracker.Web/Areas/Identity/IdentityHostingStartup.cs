using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ComicTracker.Web.Areas.Identity.IdentityHostingStartup))]

namespace ComicTracker.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
#pragma warning disable SA1500 // Braces for multi-line statements should not share line
            builder.ConfigureServices((context, services) => {
#pragma warning restore SA1500 // Braces for multi-line statements should not share line
            });
        }
    }
}
