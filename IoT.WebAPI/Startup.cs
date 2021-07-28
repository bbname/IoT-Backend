using Azure.Core;
using FluentValidation;
using IoT.Devices.Service.AzureBlobStorage;
using IoT.Devices.Service.Infrastructure;
using IoT.Devices.Service.Infrastructure.AppSettings;
using IoT.WebAPI.Infrastructure.ErrorHandling;
using IoT.WebAPI.Infrastructure.Pipelines;
using IoT.WebAPI.Infrastructure.Swagger;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace IoT.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AzureBlobStorageAppSettings>(Configuration.GetSection("AzureBlobStorage"));
            services.AddLazyCache();
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            }); ;
            services.AddMediatR(typeof(Startup), typeof(IMarkerInterface));
            services.AddValidatorsFromAssembly(typeof(IMarkerInterface).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddAutoMapper(typeof(IMarkerInterface));
            services.AddApiVersioning(config =>
            {
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            services.AddAzureClients(builder =>
            {
                var azureBlobStorageConnectionString = Configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
                builder.AddBlobServiceClient(azureBlobStorageConnectionString)
                    .ConfigureOptions(options => {
                        options.Retry.Mode = RetryMode.Exponential;
                        options.Retry.MaxRetries = 10;
                    });
            });
            services.AddScoped<IDevicesBlobStorageService, DevicesBlobStorageService>();
            services.AddScoped<IDevicesBlobStorageServiceCache, DevicesBlobStorageServiceCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", 
                            description.GroupName.ToUpperInvariant());
                    }
                });
        }
    }
}
