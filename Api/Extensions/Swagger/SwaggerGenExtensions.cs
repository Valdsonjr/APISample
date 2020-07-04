using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace Api.Extensions.Swagger
{
    public static class SwaggerGenExtensions
    {
        public static void AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();
                c.OperationFilter<SwaggerLanguageHeader>();
                var apiXML = Path.Combine(AppContext.BaseDirectory, $"Api.xml");
                var domainXML = Path.Combine(AppContext.BaseDirectory, $"Domain.xml");
                c.IncludeXmlComments(apiXML, true);
                c.AddFluentValidationRules();
            });
        }
    }
}
