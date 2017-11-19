using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace StockExchangeMachine
{
    public class StockProduct
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


        public decimal Price { get; set; }

        public void Bid(int count, decimal price, string customer)
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
        }

        private void TryBuy(Order bid)
        {
            var possibleOffers =
                this.Orders
                .Where(o => o.OrderType == OrderType.Offer)
                .Where(o => o.Price <= bid.Price)
                .OrderBy(o => o.Price)
                .OrderBy(o => o.OrderDate)
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

        public void Offer(int count, decimal price, string customer)
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
        }

        private void TrySell(Order offer)

        {
            var possibleBids =
                this.Orders
                .Where(o => o.OrderType == OrderType.Bid)
                .Where(o => o.Price >= offer.Price)
                .OrderByDescending(o => o.Price)
                .OrderBy(o => o.OrderDate)
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

        public List<Order> Orders = new List<Order>();

        public event EventHandler OrderPut;

        public event EventHandler TransactionOccured;

        private void Order_OrderTransactionsCompleted(object sender, EventArgs e)
        {
            this.Orders.Remove(sender as Order);
        }
    }


}
