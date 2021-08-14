using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using BazarCacheApi.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BazarCacheApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///     This Method gets called by the runtime
        ///     and it is used to add services to the container.
        /// </summary>
        /// <param name="services">service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // This will add the Controllers in the application 
            // the controller is defined by implementing the ControllerBase or Controller
            services.AddControllers();
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
            // Adding Dictionary as a singleton which means that all the requests will see the same object.
            services.AddSingleton<Dictionary<string, Cache>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BazarCacheApi v1"));
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BazarCacheApi v1"));
            }

            // we have removed https redirection because it has caused a problem because 
            // our certificate is self signed this can be turned on in a production environment
            // app.UseHttpsRedirection();

            // Allow Routing in our Controller
            app.UseRouting();

            // Add Authorization in our Http
            app.UseAuthorization();

            // Map The Controllers for their endpoints
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}