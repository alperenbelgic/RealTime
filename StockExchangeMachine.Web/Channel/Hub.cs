using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace StockExchangeMachine.Web.Channel
{
    public class TheHub : Hub
    {
        public IObservable<Transaction> StreamTransactions(string param)
        {
            if (param == "x")
            {
                return StockProductObservables.GetTransactions(
           Models.TempStatic.StockExchangeMachineModel.GetStockProduct("VOD"));
            }

            return StockProductObservables.GetTransactions(
            Models.TempStatic.TempStockProduct);
        }

        public IObservable<PriceModel> StreamPrices(string stockProductCode)
        {
            try
            {
                var stockProduct = Models.TempStatic.StockExchangeMachineModel.GetStockProduct(stockProductCode);

                return StockProductObservables.GetPrices(stockProduct);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IObservable<PriceInterval> StreamPriceInformation(string stockProductCode)
        {
            var stockProduct = Models.TempStatic.StockExchangeMachineModel.GetStockProduct(stockProductCode);

            if (stockProduct == null)
            {
                throw new Exception($"There is no stock product with the code: {stockProductCode}");
            }

            return stockProduct.WhenPriceInformationChanged();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

    }
}
