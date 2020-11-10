using Data.EFCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Api.Extensions
{
    /// <summary>
    /// Extensões de configuração (IOptions)
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adiciona algumas classes de configuração para injeção.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenApiInfo>(configuration.GetSection("Info"));
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionString"));
        }
    }
}
