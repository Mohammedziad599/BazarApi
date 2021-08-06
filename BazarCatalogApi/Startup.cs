using System;
using System.IO;
using System.Reflection;

using BazarCatalogApi.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace BazarCatalogApi
{
    /// <summary>
    ///     this has our configured services as well as our Injected Dependency
    ///     and finally our Http Pipeline Configuration.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     the Startup class Constructor
        /// </summary>
        /// <param name="configuration">the Application configuration in the appsettings.json</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        ///     This Method gets called by the runtime
        ///     and it is used to add services to the container.
        /// </summary>
        /// <param name="services">service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // AddCors used to add the Cors service needed because we want to
            // add the Header Allow-Control-Access-Origin to the response.
            services.AddCors();
            // This will add the dependency injection for the httpclient for this project.
            services.AddHttpClient();
            // This will add the DbContext for the Entity Framework Core and we
            // are using Sqlite as our persistant data storage.
            services.AddDbContext<CatalogContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("ConnectionString"));
            });
            // This will add the Controllers in the application 
            // the controller is defined by implementing the ControllerBase or Controller 
            // also we have added NewtonsoftJson with a Camel Case Contract Resolver used in 
            // AutoMapper
            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            // Adding Swagger Documentation and Configuring Swagger.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Bazar Catalog API",
                    Description = "A book catalog api",
                    Contact = new OpenApiContact
                    {
                        Name = "Mohammed Ziad",
                        Email = "Mohammedziad599@gmail.com",
                        Url = new Uri("https://github.com/Mohammedziad599")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            // Add AutoMapper to the container.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Inject SqlCatalogRepo everytime a class depend on ICatalogRepo
            // Scoped is the lifetime of the service which means that the service is created once
            // per client request which means if 10 clients wants has sent a request 
            // there will be 10 different created to handle each request.
            services.AddScoped<ICatalogRepo, SqlCatalogRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BazarCatalogApi v1"));
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BazarCatalogApi v1"));
            }

            // we have removed https redirection because it has caused a problem because 
            // our certificate is self signed this can be turned on in a production environment
            // app.UseHttpsRedirection();

            // Allow Routing in our Controller
            app.UseRouting();

            // Allow Any Origin and Any Method in the CORS (Cross Origin Requests)
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });

            // Add Authorization in our Http
            app.UseAuthorization();

            // Map The Controllers for their endpoints
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}