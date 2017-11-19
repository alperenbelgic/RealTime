using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace StockExchangeMachine.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            System.Diagnostics.Trace.WriteLine("funk!");

            var stockProduct = new StockProduct() { StockProductCode = "google" };

            var transactions = new List<Transaction>();

            stockProduct.TransactionOccured += (object o, EventArgs e) =>
            {
                transactions.Add(o as Transaction);
            };


            StockProductObservables.GetTransactions(stockProduct);
            StockProductObservables.GetPrices(stockProduct);

            stockProduct.Offer(100, 5, "seller 1");
            stockProduct.Bid(40, 6, "buyer 1");

            {
                Assert.AreEqual(1, transactions.Count);
                Assert.AreEqual(40, transactions[0].Count);
                Assert.AreEqual(5, transactions[0].Price);
                Assert.AreEqual("buyer 1", transactions.First().Buyer);
                Assert.AreEqual("seller 1", transactions.First().Seller);
            }

            stockProduct.Bid(120, 5.5m, "buyer 2");

            {
                Assert.AreEqual(2, transactions.Count);
                Assert.AreEqual(60, transactions[1].Count);
                Assert.AreEqual(5, transactions[1].Price);
                Assert.AreEqual("seller 1", transactions[1].Seller);
                Assert.AreEqual("buyer 2", transactions[1].Buyer);
            }

            stockProduct.Bid(50, 5.5m, "buyer 3");
            stockProduct.Bid(20, 5, "buyer 4");
            stockProduct.Offer(120, 5, "seller 2");

            {
                Assert.AreEqual(5, transactions.Count);

                Assert.AreEqual(60, transactions[2].Count);
                Assert.AreEqual("buyer 2", transactions[2].Buyer);
                Assert.AreEqual("seller 2", transactions[2].Seller);
                Assert.AreEqual(5.5m, transactions[2].Price);

                var transaction4 = transactions[3];
                Assert.AreEqual(50, transaction4.Count);
                Assert.AreEqual("buyer 3", transaction4.Buyer);
                Assert.AreEqual("seller 2", transaction4.Seller);
                Assert.AreEqual(5.5m, transaction4.Price);

                var transaction5 = transactions[4];
                Assert.AreEqual(10, transaction5.Count);
                Assert.AreEqual("buyer 4", transaction5.Buyer);
                Assert.AreEqual("seller 2", transaction5.Seller);
                Assert.AreEqual(5m, transaction5.Price);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestRandomPricing()
        {
            var stockProduct = new StockProduct() { StockProductCode = "google" };
            stockProduct.Price = 100;

            List<decimal> list = new List<decimal>();

            for (int i = 0; i < 100000; i++)
            {
                var rog = new RandomOrderGenerator(stockProduct);
                rog.GenerateOrder();

                //System.Diagnostics.Trace.WriteLine(stockProduct.Price);

                list.Add(stockProduct.Price);


            }

            Trace.WriteLine("average " + list.Average());


            //System.Reactive.Linq.Observable.Timer()

            var a = new List<int>() { 1 };


        }

        [TestMethod]
        public void TestRandomPricingRandomInterval()
        {
            var stockProduct = new StockProduct() { StockProductCode = "google" };
            stockProduct.Price = 100;

            DateTime d = DateTime.Now;

            RandomOrderInterval.GetObservable()
                .Subscribe(
                onNext: u =>
                {
                    var rog = new RandomOrderGenerator(stockProduct);

                    rog.GenerateOrder();

                    var ts = DateTime.Now - d;

                    System.Diagnostics.Trace.WriteLine(ts);
                    System.Diagnostics.Trace.WriteLine(stockProduct.Price);
                    System.Diagnostics.Trace.WriteLine(" ");

                }
                );

            Task.Delay(100000);

        }

    }
}