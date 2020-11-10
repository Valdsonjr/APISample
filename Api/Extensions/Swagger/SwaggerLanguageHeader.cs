using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Api.Extensions.Swagger
{

    /// <summary>
    /// Adiciona um header para seleção de linguagem em TODOS os endpoints
    /// 
    /// https://dejanstojanovic.net/aspnet/2019/april/localization-of-the-dtos-in-a-separate-assembly-in-aspnet-core/
    /// </summary>
    public class SwaggerLanguageHeader : IOperationFilter
    {
        readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public SwaggerLanguageHeader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var schema = new OpenApiSchema { Type = "string", Description = "Language" };

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Description = "Supported languages",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = (_serviceProvider.GetService(typeof(IOptions<RequestLocalizationOptions>)) as IOptions<RequestLocalizationOptions>)?
                        .Value?.SupportedCultures?.Select(c => OpenApiAnyFactory.CreateFor(schema, c.Name)).ToList()
                },
                Required = false
            });
        }
    }
}
