using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BazarCatalogApi
{
    /// <summary>
    ///     This is the First Class that will be Executed by Dotnet
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///     this will create a hostBuilder which can be used to build and run the application
        ///     the hostBuilder will use the Startup Class in the File Startup.cs
        ///     which will have our services Configuration and our Http Pipeline Configuration.
        /// </summary>
        /// <param name="args">program argument</param>
        /// <returns>a host builder that will be used to build and run the app</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}