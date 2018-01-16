using System;
using System.Collections.Generic;
using System.Linq;
using IgMarketsDataDownloader.Configuration;
using IgMarketsDataDownloader.Entity;
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
        private readonly List<Instrument> _tradableInstruments;

        public DownloadingService(
            AppSettings configuration, 
            IMemoryCache memoryCache,
            List<Instrument> tradableInstruments
            )
        {
            Configuration = configuration;
            _memoryCache = memoryCache;
            _tradableInstruments = tradableInstruments;
        }

        public void Start()
        {
            ObtainOAuthToken();

            var xjo = _tradableInstruments.First(ins => ins.Name == "S&P/ASX 200");
        }

        private void ObtainOAuthToken()
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
            request.AddHeader("VERSION", "3");
            var response = client.Execute<AuthenticationResponse>(request);
            var content = response.Data;
            _memoryCache.Set("OAuth.AccessToken", content.OauthToken.AccessToken, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(content.OauthToken.ExpiresIn - 5)));
            _memoryCache.Set("OAuth.RefreshToken", content.OauthToken.RefreshToken, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(content.OauthToken.ExpiresIn - 5)));
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
