using System;

namespace StockExchangeMachine
{
    public class Transaction : Profilable
    {
        public Transaction()
        {
        }

        public string StockProductCode { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string Buyer { get; set; }
        public string Seller { get; set; }
        public DateTime TransactionTime { get; set; }

        public override string ToString()
        {
            var text = $"Buyer: {Buyer}, Seller: {Seller}, Price: {Price}, Count: {Count} ";

            return text;
        }
    }
}