using System.Collections.Generic;
using IgMarketsDataDownloader.Entity.Price;

namespace IgMarketsDataDownloader.ResponseModel
{
    public class PriceListResponse
    {
        ///<Summary>
        ///Price list
        ///</Summary>
        public List<PriceSnapshot> Prices { get; set; }

        ///<Summary>
        ///the instrument type of this instrument
        ///</Summary>
        public string instrumentType { get; set; }

        ///<Summary>
        ///Historical price data allowance
        ///</Summary>
        public Metadata Metadata { get; set; }
    }
}
