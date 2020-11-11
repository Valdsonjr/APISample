using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Api.Extensions
{
    /// <summary>
    /// Métodos de extensão para configuração da serialização
    /// </summary>
    public static class SerializerExtensions
    {
        /// <summary>
        /// Configuração de serialização de JSON
        /// </summary>
        /// <param name="mvcBuilder"></param>
        public static IMvcBuilder AddCustomJSONOptions(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            return mvcBuilder;
        }
    }
}
