namespace IgMarketsDataDownloader.Entity.Authorization
{
    public class AccountInfo
    {
        ///<Summary>
        ///Balance of funds in the account
        ///</Summary>
        public decimal? Balance { get; set; }

        ///<Summary>
        ///Minimum deposit amount required for margins
        ///</Summary>
        public decimal? Deposit { get; set; }

        ///<Summary>
        ///Account profit and loss amount
        ///</Summary>
        public decimal? ProfitLoss { get; set; }

        ///<Summary>
        ///Account funds available for trading amount
        ///</Summary>
        public decimal? Available { get; set; }
    }
}
