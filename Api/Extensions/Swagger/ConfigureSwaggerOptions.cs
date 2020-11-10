using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


/* ESSA CLASSE FOI COPIADA DO LINK ABAIXO, COM ALGUMAS ALTERAÇÕES
 * NAS INFORMAÇÕES DA API E DO AUTOR:
 * https://github.com/microsoft/aspnet-api-versioning
 */

namespace Api.Extensions.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly OpenApiInfo _info;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        /// <param name="info"></param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptions<OpenApiInfo> info)
        {
            _provider = provider;
            _info = info.Value;
        }

        void IConfigureOptions<SwaggerGenOptions>.Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, _info));
            }
        }
        
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, OpenApiInfo _info)
        {
            _info.Version = description.ApiVersion.ToString();

            if (description.IsDeprecated)
            {
                _info.Description += " This version of the API is obsolete.";
            }

            return _info;
        }
    }
}