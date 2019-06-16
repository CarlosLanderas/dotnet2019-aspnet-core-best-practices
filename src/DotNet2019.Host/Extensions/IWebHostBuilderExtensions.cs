using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHost MigrateDatabase<TContext>(this IWebHost webHost) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TContext>();
                context.Database.Migrate();
            }

            return webHost;
        }
    }
}
