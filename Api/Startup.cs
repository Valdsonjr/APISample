using Api.Extensions;
using Api.Extensions.Swagger;
using Domain.UnitOfWork;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Json.Serialization;
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
         * mas assim que possível, remover essa função
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
            .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IUnitOfWork>())
            .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddCustomLocalization();

            if (Environment.IsDevelopment())
            {
                services.AddDevelopmentLogging();
                services.AddDevelopmentServices();
            }

            if (Environment.IsStaging() || Environment.IsProduction())
            {
                services.AddStagingServices();
            }

            services.AddServices();

            services.AddHealthChecks();

            services.AddCustomVersioning();

            services.AddCustomSwaggerGen();
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
