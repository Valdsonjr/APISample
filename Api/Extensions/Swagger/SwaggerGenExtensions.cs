using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace Api.Extensions.Swagger
{
    /// <summary>
    /// Extensões do SwaggerGen
    /// </summary>
    public static class SwaggerGenExtensions
    {
        /// <summary>
        /// Configuração básica do SwaggerGen
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();
                c.OperationFilter<SwaggerLanguageHeader>();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Api.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Domain.xml"));
            });
        }
    }
}
