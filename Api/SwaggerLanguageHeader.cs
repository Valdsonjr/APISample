using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

/* Adiciona um header para seleção de linguagem em TODOS os endpoints
 * https://dejanstojanovic.net/aspnet/2019/april/localization-of-the-dtos-in-a-separate-assembly-in-aspnet-core/
 */

namespace Api
{
#pragma warning disable CS1591 // O comentário XML ausente não foi encontrado para o tipo ou membro visível publicamente
    public class SwaggerLanguageHeader : IOperationFilter
    {
        readonly IServiceProvider serviceProvider;
        public SwaggerLanguageHeader(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var schema = new OpenApiSchema { Type = "string", Description = "Language" };

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Description = "Linguagens suportadas",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = (serviceProvider.GetService(typeof(IOptions<RequestLocalizationOptions>)) as IOptions<RequestLocalizationOptions>)?
                        .Value?.SupportedCultures?.Select(c => OpenApiAnyFactory.CreateFor(schema, c.Name)).ToList()
                },
                Required = false
            });
        }
    }
#pragma warning restore CS1591 // O comentário XML ausente não foi encontrado para o tipo ou membro visível publicamente
}
