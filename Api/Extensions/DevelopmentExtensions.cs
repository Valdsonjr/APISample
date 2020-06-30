using Data.Mock.Repositorios;
using Domain.Repositorios;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<IItemRepository, MockItemRepository>();
        }
    }
}
