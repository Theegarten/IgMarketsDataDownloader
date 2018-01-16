namespace IgMarketsDataDownloader.Entity.Price
{
    public class PriceSnapshot
    {
        /// <Summary>
        ///     Snapshot time, format is yyyy/MM/dd hh:mm:ss
        /// </Summary>
        public string SnapshotTime { get; set; }

        /// <Summary>
        ///     Open price
        /// </Summary>
        public Price OpenPrice { get; set; }

        /// <Summary>
        ///     Close price
        /// </Summary>
        public Price ClosePrice { get; set; }

        /// <Summary>
        ///     High price
        /// </Summary>
        public Price HighPrice { get; set; }

        /// <Summary>
        ///     Low price
        /// </Summary>
        public Price LowPrice { get; set; }

        /// <Summary>
        ///     Last traded volume. This will generally be null for non exchange-traded instruments
        /// </Summary>
        public decimal? LastTradedVolume { get; set; }
    }
}