﻿using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    /// <summary>
    /// Extensões de versionamento
    /// </summary>
    public static class VersioningExtensions
    {
        /// <summary>
        /// Configuração básica de versionamento
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCustomVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(c => c.ReportApiVersions = true)
                    .AddVersionedApiExplorer(c =>
                    {
                        c.GroupNameFormat = "'v'VVV";
                        c.SubstituteApiVersionInUrl = true;
                    });

            return services;
        }
    }
}
