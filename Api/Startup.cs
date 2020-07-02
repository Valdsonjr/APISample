using Api.Extensions;
using Domain.Validadores;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
#pragma warning disable CS1591 // O comentário XML ausente não foi encontrado para o tipo ou membro visível publicamente
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        /* JSONPatch ainda usa o newtonsoft e não sabemos quando vai deixar de usar, vide:
         * https://github.com/dotnet/aspnetcore/issues/16968
         * mas assim que possível, remover essa função e o .AddNewtonsoftJson()
         * lá embaixo.
        */ 
        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ItemValidator>())
            .AddNewtonsoftJson()
            .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddLogging(conf => conf.AddConsole());
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

            if (Environment.IsDevelopment())
                services.AddDevelopmentServices();

            if (Environment.IsStaging() || Environment.IsProduction())
                services.AddStagingServices();

            services.AddServices();

            services.AddApiVersioning(c => c.ReportApiVersions = true);
            services.AddVersionedApiExplorer(c =>
            {
                c.GroupNameFormat = "'v'VVV";
                c.SubstituteApiVersionInUrl = true;
            });

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              ILogger<Startup> logger, 
                              IApiVersionDescriptionProvider provider)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (Environment.IsProduction()  || Environment.IsStaging())
            {
                app.UseResponseCaching();
                app.UseExceptionHandler(options => options.Run(async context =>
                {
                    logger.LogError(context.Features.Get<IExceptionHandlerPathFeature>().Error.Message);
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await Task.CompletedTask;
                }));
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                c.RoutePrefix = string.Empty;
            });

            app.UseRequestLocalization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
#pragma warning restore CS1591 // O comentário XML ausente não foi encontrado para o tipo ou membro visível publicamente
}
