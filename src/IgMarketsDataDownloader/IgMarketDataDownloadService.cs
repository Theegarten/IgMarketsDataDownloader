using System;
using DasMulli.Win32.ServiceUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IgMarketsDataDownloader
{
    internal class IgMarketDataDownloadService : IWin32Service
    {
        private readonly string[] commandLineArguments;
        private IWebHost webHost;
        private ILogger _logger;
        private bool stopRequestedByWindows;
        private ServiceProvider Container;

        public IgMarketDataDownloadService(string[] commandLineArguments)
        {
            this.commandLineArguments = commandLineArguments;
        }

        public string ServiceName => "IG Market Data Download Service";

        public void Start(string[] startupArguments, ServiceStoppedCallback serviceStoppedCallback)
        {
            // in addition to the arguments that the service has been registered with,
            // each service start may add additional startup parameters.
            // To test this: Open services console, open service details, enter startup arguments and press start.
            string[] combinedArguments;
            if (startupArguments.Length > 0)
            {
                combinedArguments = new string[commandLineArguments.Length + startupArguments.Length];
                Array.Copy(commandLineArguments, combinedArguments, commandLineArguments.Length);
                Array.Copy(startupArguments, 0, combinedArguments, commandLineArguments.Length, startupArguments.Length);
            }
            else
            {
                combinedArguments = commandLineArguments;
            }

            var config = new ConfigurationBuilder()
                .AddCommandLine(combinedArguments)
                .Build();

            webHost = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .UseConfiguration(config)
                .Build();

            // Make sure the windows service is stopped if the
            // ASP.NET Core stack stops for any reason
            webHost
                .Services
                .GetRequiredService<IApplicationLifetime>()
                .ApplicationStopped
                .Register(() =>
                {
                    if (stopRequestedByWindows == false)
                    {
                        serviceStoppedCallback();
                    }
                });
            
            webHost.Start();
            var serviceScopeFactory = webHost.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var processor = scope.ServiceProvider.GetService<IDownloadingService>();
                processor.Start();
            }
        }
        public void Stop()
        {
            stopRequestedByWindows = true;
            webHost.Dispose();
        }
    }

    internal class AspNetCoreStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            //todo to implement            
        }
    }
}
