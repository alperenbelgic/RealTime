using System;

namespace StockExchangeMachine
{
    public class Price
    {
        public string StockProductCode { get; internal set; }
        public decimal PriceValue { get; internal set; }
        public DateTime UpdateDate { get; internal set; }
    }
}