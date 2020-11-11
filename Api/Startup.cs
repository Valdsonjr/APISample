using Api.Extensions;
using Api.Extensions.Swagger;
using AutoMapper;
using Data.EFCore.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                    .AddCustomJSONOptions()
                    .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddDbContextPool<ItemContext>(options => 
                options.UseSqlServer(Configuration["ConnectionStrings:ItemDb"]));

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

            services.AddConfiguration(Configuration);

            services.AddServices();

            services.AddCustomHealthChecks();

            services.AddCustomVersioning();

            services.AddCustomSwaggerGen();

            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            if (Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/api/v1/Error/Development");
            }
            else
            {
                app.UseResponseCaching();
                app.UseExceptionHandler("/api/v1/Error/Production");
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
