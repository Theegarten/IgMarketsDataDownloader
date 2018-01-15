namespace IgMarketsDataDownloader.RequestModel
{
    public class AuthenticationRequest
    {
        /// <Summary>
        ///     Client login identifier
        /// </Summary>
        public string Identifier { get; set; }

        /// <Summary>
        ///     Client login password
        /// </Summary>
        public string Password { get; set; }
    }
}