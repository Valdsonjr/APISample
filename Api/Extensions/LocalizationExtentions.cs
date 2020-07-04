using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

namespace Api.Extensions
{
    /// <summary>
    /// Extensões para localização
    /// </summary>
    public static class LocalizationExtentions
    {
        /// <summary>
        /// Configuração básica de localização
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomLocalization(this IServiceCollection services)
        {
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("pt-br"),
                        new CultureInfo("en-us")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("pt-br", "pt-br");
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });
        }
    }
}
