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
