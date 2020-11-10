﻿using Api.Infrastructure;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace Api.Extensions
{
    /// <summary>
    /// Extensões de serviços
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adiciona os serviços na injeção de dependência
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>()
                    .AddTransient<Monitoring>()
                    .AddMvc(options =>
                    {
                        options.Filters.AddService<Monitoring>();
                    });
        }
    }
}
