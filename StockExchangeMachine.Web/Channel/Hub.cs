using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace StockExchangeMachine.Web.Channel
{
    public class TheHub : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }



        public Task RegisterForPriceChange(string productCode)
        {
            return
            this.Groups.AddAsync(
            this.Context.ConnectionId,
            "product$" + productCode);
        }

        public IObservable<Transaction> StreamTransactions(string param)
        {
            return StockProductObservables.GetTransactions(
            Models.TempStatic.TempStockProduct);
        }

        public IObservable<Price> StreamPrices(string stockProductCode)
        {
            var stockProduct = Models.TempStatic.StockExchangeMachineModel.GetStockProduct(stockProductCode);

            return StockProductObservables.GetPrices(stockProduct);
        }

        //public Task UpdatePrice(string productCode, decimal price)
        //{
        //    //return Clients.All.InvokeAsync("UpdatePrice", $"price changed {price}");
        //    return Clients.Group("product$"+ productCode).InvokeAsync("UpdatePrice", $"price changed {price}");
        //}

        public Task UpdatePrice(decimal price)
        {
            return Clients.All.InvokeAsync("UpdatePrice", $"price changed {price}");
        }



        public Task TransactionDone(string transactionString)
        {
            return Clients.All.InvokeAsync("TransactionDone", $" Trans: {transactionString} ");
        }

    }
}
