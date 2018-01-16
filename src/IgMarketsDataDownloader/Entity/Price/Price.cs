namespace IgMarketsDataDownloader.Entity.Price
{
    public class Price
    {
        ///<Summary>
        ///Bid price
        ///</Summary>
        public decimal? Bid { get; set; }

        ///<Summary>
        ///Ask price
        ///</Summary>
        public decimal? Ask { get; set; }

        ///<Summary>
        ///Last traded price.  This will generally be null for non exchange-traded instruments
        ///</Summary>
        public decimal? LastTraded { get; set; }

        public decimal? SpotPrice { get; set; }
    }
}
