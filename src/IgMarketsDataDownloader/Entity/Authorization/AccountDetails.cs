namespace IgMarketsDataDownloader.Entity.Authorization
{
    public class AccountDetails
    {
        ///<Summary>
        ///Account identifier
        ///</Summary>
        public string AccountId { get; set; }

        ///<Summary>
        ///Account name
        ///</Summary>
        public string AccountName { get; set; }

        ///<Summary>
        ///Indicates whether this account is the client's preferred account
        ///</Summary>
        public bool Preferred { get; set; }

        ///<Summary>
        ///Account type
        ///</Summary>
        public string AccountType { get; set; }
    }
}
