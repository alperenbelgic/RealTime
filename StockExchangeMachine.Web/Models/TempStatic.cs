using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeMachine.Web.Models
{
    public static class TempStatic
    {
        public static async void Initialise()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:55497/TheHub")
                .Build();

            await connection.StartAsync();



            //        StockProductObservables.GetTransactions(TempStockProduct).Subscribe(
            //transaction =>
            //{
            //    connection.InvokeAsync("TransactionDone", transaction.ToString());
            //}
            //);

            StockProductObservables.GetPrices(TempStockProduct).Subscribe(
                 price =>
                 {
                     connection.InvokeAsync("UpdatePrice", price.PriceValue);
                 });


        }

        static StockProduct _TempStockProduct = null;
        public static StockProduct TempStockProduct
        {
            get
            {
                if (_TempStockProduct == null)
                {
                    _TempStockProduct = new StockProduct();
                    Initialise();
                }

                return _TempStockProduct;
            }
        }

        public static StockExchangeMachineModel StockExchangeMachineModel
        {
            get
            {
                if (_StockExchangeMachineModel == null)
                {
                    _StockExchangeMachineModel = new StockExchangeMachineModel();
                    InitialiseStocks();
                }
                return _StockExchangeMachineModel;
            }
        }

        private static void InitialiseStocks()
        {
            _StockExchangeMachineModel.AddStockProduct(
                new StockProduct()
                {
                    StockProductCode = "RBS.L"
                });

            _StockExchangeMachineModel.AddStockProduct(
                new StockProduct()
                {
                    StockProductCode = "TSCO.L"
                });



        }

        private static StockExchangeMachineModel _StockExchangeMachineModel = null;

    }
}
