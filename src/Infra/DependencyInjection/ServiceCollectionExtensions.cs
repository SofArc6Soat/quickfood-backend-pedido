using Infra.Context;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infra.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddInfraDependencyServices(this IServiceCollection services, string connectionString, bool useInMemoryDatabase = false)
        {
            if (useInMemoryDatabase)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString,
                                         o => { o.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null); }));
            }

            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
        }
    }
}