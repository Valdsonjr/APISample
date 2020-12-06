using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Api.Extensions.Swagger
{
    /// <summary>
    /// Filtro para exibição correta por parte do swagger de endpoints que necessitam de autenticação.
    /// </summary>
    public class SwaggerBearerAuthentication : IOperationFilter
    {
        void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var allowAnonymous = context.ApiDescription.CustomAttributes().Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));
            var authRequired   = context.ApiDescription.CustomAttributes().Any(attr => attr.GetType() == typeof(AuthorizeAttribute));

            if (allowAnonymous || !authRequired) return;

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "JWT Authentication"
                            }
                        },
                        new List<string>()
                    }
                }
            };
        }
    }
}