using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchangeMachine
{
    public class Order
    {
        public int Count { get; internal set; }
        public int RemainingCount { get { return Count - Transactions.Sum(t => t.Count); } }
        public decimal Price { get; internal set; }
        public string Customer { get; internal set; }
        public DateTime OrderDate { get; internal set; }
        public OrderType OrderType { get; set; }
        public string StockProductCode { get; internal set; }

        List<Transaction> Transactions = new List<Transaction>();

        internal void AddTransaction(Transaction transaction)
        {
            if (RemainingCount >= transaction.Count)
            {
                this.Transactions.Add(transaction);

                if (this.RemainingCount == 0)
                {
                    OrderTransactionsCompleted?.Invoke(this, null);
                }
            }
        }

        public event EventHandler OrderTransactionsCompleted;
    }

    public enum OrderType
    {
        Bid,
        Offer
    }

}