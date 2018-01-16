namespace IgMarketsDataDownloader.Configuration
{
    public class AppSettings
    {
        public Profile Profile { get; set; }
        
        public RestEndpoint RestEndpoint { get; set; }
    }

    public class Profile
    {
        public string Environment { get; set; }

        public string DemoBaseUrl { get; set; }

        public string LiveBaseUrl { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ApiKey { get; set; }
    }
}