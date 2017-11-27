using System;

namespace StockExchangeMachine
{
    public class PriceModel : Profilable
    {
        public string StockProductCode { get; internal set; }
        public decimal Price { get; internal set; }
        public DateTime UpdateDate { get; internal set; }
    }
}