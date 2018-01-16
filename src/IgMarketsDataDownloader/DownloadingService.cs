using System;
using System.Collections.Generic;
using System.Linq;
using IgMarketsDataDownloader.Configuration;
using IgMarketsDataDownloader.Entity;
using IgMarketsDataDownloader.Entity.Price;
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
            ObtainPriceHistory();
        }

        private void ObtainPriceHistory()
        {
            var xjo = _tradableInstruments.First(ins => ins.Name == "S&P/ASX 200");
            var url = Configuration.Profile.Environment == "live" ? Configuration.Profile.LiveBaseUrl : Configuration.Profile.DemoBaseUrl;

            var from = new DateTime(2018, 01, 16, 15, 55, 00).ToString("yyyy-MM-ddTHH:mm:ss");
            var to = new DateTime(2018, 01, 16, 16, 05, 00).ToString("yyyy-MM-ddTHH:mm:ss");

            var client = new RestClient(url);
            var request = new RestRequest(string.Format(Configuration.RestEndpoint.Prices.PriceByStartDateAndEndDate, xjo.Identifier, Resolution.MINUTE.ToString(), from, to))
            {
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };

            string accountId;
            string token;

            _memoryCache.TryGetValue("AccountId", out accountId);
            _memoryCache.TryGetValue("OAuth.AccessToken", out token);

            request.AddBody(new AuthenticationRequest { Identifier = Config.Get("IgMarketsUserName"), Password = Config.Get("IgMarketsPassword") });
            request.AddHeader(Constants.IgApiKeyHeader, Config.Get("IgMarketsApiKey"));
            request.AddHeader("VERSION", "3");
            request.AddHeader("IG-ACCOUNT-ID", accountId);
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = client.Execute<PriceListResponse>(request);
            foreach (var price in response.Data.Prices)
            {
                price.OpenPrice.SpotPrice = (price.OpenPrice.Ask + price.OpenPrice.Bid) / 2;
                price.HighPrice.SpotPrice = (price.HighPrice.Ask + price.HighPrice.Bid) / 2;
                price.LowPrice.SpotPrice = (price.LowPrice.Ask + price.LowPrice.Bid) / 2;
                price.ClosePrice.SpotPrice = (price.ClosePrice.Ask + price.ClosePrice.Bid) / 2;
            }
            
            ///persist this into file location.

        }

        private void ObtainOAuthToken()
        {
            var url = Configuration.Profile.Environment == "live" ? Configuration.Profile.LiveBaseUrl : Configuration.Profile.DemoBaseUrl;

            var client = new RestClient(url);
            var request = new RestRequest(Configuration.RestEndpoint.Logins.Session)
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
            _memoryCache.Set("AccountId", content.AccountId);
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
