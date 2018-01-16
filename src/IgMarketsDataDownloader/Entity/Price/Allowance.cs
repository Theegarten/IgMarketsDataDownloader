namespace IgMarketsDataDownloader.Entity.Price
{
    public class Allowance
    {
        /// <Summary>
        ///     The number of data points still available to fetch within the current allowance period
        /// </Summary>
        public int RemainingAllowance { get; set; }

        /// <Summary>
        ///     The number of data points the API key and account combination is allowed to fetch in any given allowance period
        /// </Summary>
        public int TotalAllowance { get; set; }

        /// <Summary>
        ///     The number of seconds till the current allowance period will end and the remaining allowance field is reset
        /// </Summary>
        public int AllowanceExpiry { get; set; }
    }
}