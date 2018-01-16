using System.IO;
using System.Linq;
using IgMarketsDataDownloader.Configuration;
using IgMarketsDataDownloader.Entity;
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
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("instrumentEpicMapping.json", false, true);

            services.AddTransient<IDownloadingService, DownloadingService>();
            ConfigurationRoot = configurationBuilder.Build();
            services.AddSingleton(provider => ConfigurationRoot);

            var appConfig = new AppSettings();
            ConfigurationRoot.GetSection("AppSettings").Bind(appConfig);
            services.AddSingleton(appSettings => appConfig);

            var instruments = ConfigurationRoot.GetSection("Instruments").Get<Instrument[]>().ToList();
            services.AddSingleton(instrumentList => instruments);

            services.AddMemoryCache();
            SimpleJson.SimpleJson.CurrentJsonSerializerStrategy = new LowerCaseJsonSerializerStrategy();
        }

        public void Configure()
        {
            
        }
    }
}
