using System.IO;
using IgMarketsDataDownloader.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IgMarketsDataDownloader
{
    public class Startup
    {
        public static IConfiguration ConfigurationRoot { get; set; }
        
        
        public void ConfigureServices(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",  false,  true);

            services.AddTransient<IDownloadingService, DownloadingService>();
            ConfigurationRoot = configurationBuilder.Build();
            services.AddSingleton(provider => ConfigurationRoot);

            var appConfig = new AppSettings();
            ConfigurationRoot.GetSection("AppSettings").Bind(appConfig);
            services.AddSingleton(appSettings => appConfig);
            services.AddMemoryCache();
            SimpleJson.SimpleJson.CurrentJsonSerializerStrategy = new LowerCaseJsonSerializerStrategy();
        }

        public void Configure()
        {
            
        }
    }
}
