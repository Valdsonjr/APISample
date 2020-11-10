using Api.Infrastructure;
using Data.EFCore.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    /// <summary>
    /// Extensões para checagem de saúde da API
    /// </summary>
    public static class HealthCheckExtensions
    {
        /// <summary>
        /// Adiciona as checagens de saúde
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddCheck<EFCoreHealthCheck<ItemContext>>(nameof(ItemContext));
        }
    }
}
