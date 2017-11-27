using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reactive.Threading;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace StockExchangeMachine
{
    public class StockProduct : Profilable
    {
        public StockProduct()
        {
            this.TransactionOccured += StockProduct_TransactionOccured;
        }

        private void StockProduct_TransactionOccured(object sender, EventArgs e)
        {
            this.Price = (sender as Transaction).Price;
        }

        public string StockProductCode { get; set; }


        decimal? InitialPrice = null;
        decimal _Price = 0;
        public decimal Price
        {
            get { return _Price; }
            set
            {
                if (InitialPrice == null)
                {
                    InitialPrice = value;
                }
                _Price = value;
            }
        }
        public decimal? LowestOfferPrice { get; private set; }
        public decimal? HighestBidPrice { get; private set; }
        public decimal GetChangePercentage(decimal price)
        {
            return decimal.Round((price - InitialPrice.Value) / InitialPrice.Value, 2);
        }

        public void Bid(int count, decimal price, string customer = "")
        {
            var order = new Order()
            {
                Count = count,
                Price = price,
                Customer = customer,
                StockProductCode = this.StockProductCode,
                OrderType = OrderType.Bid,
                OrderDate = DateTime.UtcNow
            };

            order.OrderTransactionsCompleted += Order_OrderTransactionsCompleted;

            this.Orders.Add(order);

            OrderPut?.Invoke(order, null);

            TryBuy(order);

            BroadcastLatestPriceInformation();
        }

        private void TryBuy(Order bid)
        {
            var possibleOffers =
                this.Orders
                .Where(offer => offer.OrderType == OrderType.Offer)
                .Where(offer => offer.Price <= bid.Price)
                .OrderBy(offer => offer.Price)
                .ThenBy(offer => offer.OrderDate)
                .ToList();

            foreach (var offer in possibleOffers)
            {
                Transaction transaction;

                if (bid.RemainingCount <= offer.RemainingCount)
                {
                    transaction =
                        new Transaction()
                        {
                            StockProductCode = this.StockProductCode,
                            Count = bid.RemainingCount,
                            Price = offer.Price,
                            Buyer = bid.Customer,
                            Seller = offer.Customer,
                            TransactionTime = DateTime.UtcNow
                        };

                    bid.AddTransaction(transaction);
                    offer.AddTransaction(transaction);

                    TransactionOccured?.Invoke(transaction, null);

                    break;
                }
                else if (bid.RemainingCount > offer.RemainingCount)
                {
                    transaction =
                       new Transaction()
                       {
                           StockProductCode = this.StockProductCode,
                           Count = offer.RemainingCount,
                           Price = offer.Price,
                           Buyer = bid.Customer,
                           Seller = offer.Customer,
                           TransactionTime = DateTime.UtcNow
                       };

                    bid.AddTransaction(transaction);
                    offer.AddTransaction(transaction);

                    TransactionOccured?.Invoke(transaction, null);
                }
            }



        }

        public void Offer(int count, decimal price, string customer = "")
        {
            var order = new Order()
            {
                Count = count,
                Price = price,
                Customer = customer,
                StockProductCode = this.StockProductCode,
                OrderType = OrderType.Offer,
                OrderDate = DateTime.UtcNow
            };

            order.OrderTransactionsCompleted += Order_OrderTransactionsCompleted;

            this.Orders.Add(order);

            OrderPut?.Invoke(order, null);

            TrySell(order);

            BroadcastLatestPriceInformation();
        }

        private void TrySell(Order offer)
        {
            var possibleBids =
                this.Orders
                .Where(bid => bid.OrderType == OrderType.Bid)
                .Where(bid => bid.Price >= offer.Price)
                .OrderByDescending(bid => bid.Price)
                .ThenBy(bid => bid.OrderDate)
                .ToList();

            foreach (var bid in possibleBids)
            {
                Transaction transaction;

                if (offer.RemainingCount <= bid.RemainingCount)
                {
                    transaction =
                        new Transaction()
                        {
                            StockProductCode = this.StockProductCode,
                            Count = offer.RemainingCount,
                            Price = bid.Price,
                            Buyer = bid.Customer,
                            Seller = offer.Customer,
                            TransactionTime = DateTime.UtcNow
                        };

                    offer.AddTransaction(transaction);
                    bid.AddTransaction(transaction);

                    TransactionOccured?.Invoke(transaction, null);

                    break;
                }
                else if (offer.RemainingCount > bid.RemainingCount)
                {
                    transaction =
                       new Transaction()
                       {
                           StockProductCode = this.StockProductCode,
                           Count = bid.RemainingCount,
                           Price = bid.Price,
                           Buyer = bid.Customer,
                           Seller = offer.Customer,
                           TransactionTime = DateTime.UtcNow
                       };

                    bid.AddTransaction(transaction);
                    offer.AddTransaction(transaction);

                    TransactionOccured?.Invoke(transaction, null);
                }
            }
        }

        private void BroadcastLatestPriceInformation()
        {
            decimal? lowestOffer =
            this.Orders
                .Where(o => o.OrderType == OrderType.Offer)
                .Select(o => new decimal?(o.Price))
                .OrderBy(o => o)
                .FirstOrDefault();

            if (lowestOffer.HasValue)
            {
                this.LowestOffer?.Invoke(lowestOffer.Value, null);
            }

            decimal? highestBid =
            this.Orders
                .Where(o => o.OrderType == OrderType.Bid)
                .Select(o => new decimal?(o.Price))
                .OrderByDescending(o => o)
                .FirstOrDefault();

            if (highestBid.HasValue)
            {
                this.HighestBid?.Invoke(highestBid.Value, null);
            }

            this.PriceObservable.OnNext(this.Price);

        }

        public List<Order> Orders = new List<Order>();

        public event EventHandler OrderPut;

        public event EventHandler TransactionOccured;

        private void Order_OrderTransactionsCompleted(object sender, EventArgs e)
        {
            this.Orders.Remove(sender as Order);
        }

        private event EventHandler LowestOffer;
        private event EventHandler HighestBid;

        Subject<decimal> PriceObservable = new Subject<decimal>();
        private IObservable<decimal> WhenPriceChangedAfterAnOrder()
        {
            return PriceObservable
                .DistinctUntilChanged();
        }


        private IObservable<decimal> _WhenLowestOfferChanged = null;
        private IObservable<decimal> WhenLowestOfferChanged()
        {
            if (_WhenLowestOfferChanged == null)
            {
                _WhenLowestOfferChanged =
                    Observable.FromEventPattern(
                  e => this.LowestOffer += e,
                    e => this.LowestOffer -= e)
                    .Select(e => (decimal)e.Sender)
                    .DistinctUntilChanged()
                    .Do(nextValue =>
                    {
                        this.LowestOfferPrice = nextValue;
                    });
            }

            return _WhenLowestOfferChanged;
        }

        private IObservable<decimal> _WhenHighestBidChanged = null;
        private IObservable<decimal> WhenHighestBidChanged()
        {
            if (_WhenHighestBidChanged == null)
            {
                _WhenHighestBidChanged =
                Observable.FromEventPattern(
                     e => this.HighestBid += e,
                     e => this.HighestBid -= e)
                .Select(e => (decimal)e.Sender)
                .DistinctUntilChanged()
                .Do(nextValue =>
                {
                    this.HighestBidPrice = nextValue;
                });
            }
            return _WhenHighestBidChanged;
        }

        public IObservable<PriceInterval> WhenPriceInformationChanged()
        {
            return Observable.CombineLatest(
                WhenLowestOfferChanged(),
                WhenHighestBidChanged(),
                WhenPriceChangedAfterAnOrder(),
                (decimal latestLowestOffer, decimal latestHighestBid, decimal latestPrice) =>
                {
                    return new PriceInterval()
                    {
                        HighestBid = latestHighestBid,
                        LowestOffer = latestLowestOffer,
                        LatestPrice = latestPrice,
                        Change = GetChangePercentage(latestPrice)
                    };
                });
        }
    }

    public class PriceInterval : Profilable
    {
        public decimal LowestOffer { get; set; }

        public decimal HighestBid { get; set; }

        public decimal LatestPrice { get; set; }

        public decimal Change { get; set; }
    }
}
