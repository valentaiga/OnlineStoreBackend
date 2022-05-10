using System;
using System.Linq;
using System.Net;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nest;
using OnlineStoreBackend.Abstractions.Configs;
using OnlineStoreBackend.Abstractions.Models.Category;
using OnlineStoreBackend.Abstractions.Models.Product;
using OnlineStoreBackend.Abstractions.Services.Category;
using OnlineStoreBackend.Abstractions.Services.Product;
using OnlineStoreBackend.Abstractions.Services.Search;
using OnlineStoreBackend.Api.Models;
using OnlineStoreBackend.Api.Validators.Category;
using OnlineStoreBackend.Middlewares;
using OnlineStoreBackend.Services.Category;
using OnlineStoreBackend.Services.Product;
using OnlineStoreBackend.Services.Search;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace OnlineStoreBackend
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
            services.AddControllers();
            
            // fluent requests validation
            services.AddFluentValidation(config 
                => config.RegisterValidatorsFromAssemblyContaining<AddCategoryValidator>());
            services.Configure<ApiBehaviorOptions>(options 
                => options.InvalidModelStateResponseFactory = 
                (context) =>
            {
                var firstError = context!.ModelState.FirstOrDefault().Value!.Errors.FirstOrDefault()!.ErrorMessage;
                var response = new BadRequestResponse(firstError);
                return new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            });
            
            // services
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISearchService, SearchService>();
            
            var elasticConfig = Configuration.GetSection("elasticConfig")
                .Get<ElasticConfig>();

            // logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticConfig.Uri))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
                })
                .CreateLogger();

            // elastic
            services.AddScoped<IElasticClient>(_ =>
            {
                var settings = new ConnectionSettings(new Uri(elasticConfig.Uri))
                    .DefaultMappingFor<ProductDto>(p => p
                        .IndexName(elasticConfig.ProductsIndex))
                    .DefaultMappingFor<CategoryDto>(c => c
                        .IndexName(elasticConfig.CategoryIndex));
                return new ElasticClient(settings);
            });
            
            // swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Store", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Store v1"));
            }
        }
    }
}