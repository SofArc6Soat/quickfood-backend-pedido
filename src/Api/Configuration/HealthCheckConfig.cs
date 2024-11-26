using System.Diagnostics.CodeAnalysis;

namespace Api.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class HealthCheckConfig
    {
        public static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, string dbConnectionString)
        {
            services.AddHealthChecks()
                .AddSqlServer(dbConnectionString);

            return services;
        }
    }
}