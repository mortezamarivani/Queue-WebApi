using Gx.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gx.Core.Utilities.Extensions.Connection
{
    public static class ConnectionExtension
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection service,
            IConfiguration configuration)
        {
            service.AddDbContext<GxDbContext>(options =>
            {
                var connectionString = "ConnectionString:GxConnection:Development";
                options.UseSqlServer(configuration[connectionString]);
            });

            return service;
        }
    }
}
