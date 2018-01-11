using IgMarketsDataDownloader.Configuration;
using IgMarketsDataDownloader.RequestModel;
using IgMarketsDataDownloader.ResponseModel;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace IgMarketsDataDownloader
{
    public class DownloadingService : IDownloadingService
    {
        private readonly AppSettings Configuration;
        private readonly IMemoryCache _memoryCache;

        public DownloadingService(AppSettings configuration, IMemoryCache memoryCache)
        {
            Configuration = configuration;
            _memoryCache = memoryCache;
        }

        public void Start()
        {
            var url = Configuration.Profile.Environment == "live" ? Configuration.Profile.LiveBaseUrl : Configuration.Profile.DemoBaseUrl;
            
            var client = new RestClient(url);
            var request = new RestRequest(Configuration.RestEndpoint.Login.Session)
            {
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            request.AddBody(new AuthenticationRequest { Identifier = Config.Get("IgMarketsUserName"), Password = Config.Get("IgMarketsPassword") });
            request.AddHeader(Constants.IgApiKeyHeader, Config.Get("IgMarketsApiKey"));
            var response = client.Execute<AuthenticationResponse>(request);
            var content = response.Data;
        }

        public void Stop()
        {

        }
    }

    public interface IDownloadingService
    {
        void Start();
        void Stop();
    }
}
