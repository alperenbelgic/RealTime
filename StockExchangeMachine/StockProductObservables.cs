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


            o.Subscribe(e =>
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
            );
            return o;
        }

        public static IObservable<Price> GetPrices(StockProduct stockProduct)
        {
            var o =
            GetTransactions(stockProduct)
            .DistinctUntilChanged(t => t.Price)
            .Select(t =>
            {
                return new Price
                {
                    StockProductCode = t.StockProductCode,
                    PriceValue = t.Price,
                    UpdateDate = t.TransactionTime
                };
            });

            o.Subscribe(e =>
            {
                System.Diagnostics.Trace.WriteLine(e.PriceValue);
            }
         );
            return o;

        }
    }
}
