using Data.Mock.Repositories;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Extensions
{
    /// <summary>
    /// Classe de extensões específicas para ambiente de desenvolvimento
    /// </summary>
    public static class DevelopmentExtensions
    {
        /// <summary>
        /// Adiciona os repositórios de mock na injeção de dependência
        /// </summary>
        /// <param name="services"></param>
        public static void AddDevelopmentServices(this IServiceCollection services)
        {
            services.AddScoped<IItemRepository, MockItemRepository>();
        }

        /// <summary>
        /// Adiciona os serviços de logging específicos para desenvolvimento
        /// </summary>
        /// <param name="services"></param>
        public static void AddDevelopmentLogging(this IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole(options => options.IncludeScopes = true));
        }
    }
}
