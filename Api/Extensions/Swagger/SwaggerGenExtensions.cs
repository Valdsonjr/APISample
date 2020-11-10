using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
                c.OperationFilter<SwaggerBearerAuthentication>();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Api.xml"), true);
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http
                });
            });
        }
    }
}
