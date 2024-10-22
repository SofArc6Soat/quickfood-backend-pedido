using Api;
using Controllers;
using Gateways;
using Infra.Context;
using Infra.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace SmokeTests
{
    public class SmokeTestStartup : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureServices(
            services =>
                {
                    // Remove o contexto de banco de dados real
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Adiciona o banco de dados in-memory para testes
                    services.AddInfraDependencyServices("TestConnectionString", useInMemoryDatabase: true);
                });
    }
}