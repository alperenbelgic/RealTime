using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;


namespace StockExchangeMachine
{
    public class StockProductObservables
    {
        public static IObservable<Transaction> GetTransactions(StockProduct stockProduct)
        {
            var o = Observable.FromEventPattern(
                    h => stockProduct.TransactionOccured += h,
                    h => stockProduct.TransactionOccured -= h)
                .Select(e =>
                {
                    return e.Sender as Transaction;
                });
            return o;
        }

        public static IObservable<PriceModel> GetPrices(StockProduct stockProduct)
        {
            var o =
            GetTransactions(stockProduct)
            .DistinctUntilChanged(t => t.Price)
            .Select(t =>
            {
                return new PriceModel
                {
                    StockProductCode = t.StockProductCode,
                    Price = t.Price,
                    UpdateDate = t.TransactionTime
                };
            });

            return o;

        }
    }
}
