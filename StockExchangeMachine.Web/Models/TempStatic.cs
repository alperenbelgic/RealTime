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

            StockProductObservables.GetPrices(TempStockProduct).Subscribe(
                 price =>
                 {
                     connection.InvokeAsync("UpdatePrice", price.Price);
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

            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "LLOY", Price = 65.99M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "BARC", Price = 185M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "VOD", Price = 228.7M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "GLEN", Price = 353.45M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "ITV", Price = 150.5M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "BP.", Price = 493.1M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "HSBA", Price = 731.6M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "CNA", Price = 163.2M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "TSCO", Price = 184.7M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "WPG", Price = 407.4M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "BT.A", Price = 245.4M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "SKY", Price = 940M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "TW.", Price = 196.6M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "GKN", Price = 302.5M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "NG.", Price = 882.8M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "KGF", Price = 307.3M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "LGEN", Price = 268.9M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "MKS", Price = 301.4M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "BA.", Price = 535.5M, });
            _StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode = "OML", Price = 190.7M, });


        }


        /*
         
LLOY	Lloyds Banking Group plc	65.99	71,980,593
BARC	Barclays plc	            185	    56,100,591
VOD	    Vodafone Group plc	        228.7	41,407,840
GLEN	Glencore plc	            353.45	27,871,049
ITV	    ITV plc	                    150.5   22,031,805
BP.	    BP Plc	                    493.1	15,143,602
HSBA	HSBC Holdings plc	        731.6	14,530,217
CNA	    Centrica plc	            163.2	14,059,257
TSCO	Tesco plc	                184.7	12,719,968
WPG	    Worldpay Group plc	        407.4	12,328,686
BT.A	BT Group plc	            245.4	11,932,249
SKY	    Sky plc	                    940	    9,876,569
TW.	    Taylor Wimpey plc	        196.6	9,216,791
GKN	    GKN plc	                    302.5	8,983,017
NG.	    National Grid	            882.8	8,151,343
KGF	    Kingfisher	                307.3	7,181,748
LGEN	Legal & General Group plc	268.9	7,030,232
MKS	    Marks & Spencer Group plc	301.4	5,985,654
BA.	    BAE Systems plc	            535.5	5,967,881
OML	    Old Mutual Plc	            190.7	5,911,117


_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="LLOY", Price= 65.99	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="BARC", Price= 185	    m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="VOD", Price= 228.7	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="GLEN	", Price= 353.45	m,  });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="ITV", Price= 150.5    m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="BP.", Price= 493.1	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="HSBA", Price= 731.6	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="CNA	", Price= 163.2	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="TSCO", Price= 184.7	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="WPG", Price= 407.4	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="BT.A	", Price= 245.4	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="SKY", Price= 940	    m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="TW.", Price= 196.6	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="GKN", Price= 302.5	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="NG.", Price= 882.8	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="KGF", Price= 307.3	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="LGEN", Price= 268.9	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="MKS", Price= 301.4	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="BA.", Price= 535.5	m,      });
_StockExchangeMachineModel.AddStockProduct(new StockProduct() { StockProductCode="OML", Price= 190.7	m,      });


 */


        public static void StartGeneratingOrders()
        {

            foreach (var product in StockExchangeMachineModel.StockProducts)
            {
                var rog = new RandomOrderGenerator(
           Models.TempStatic.StockExchangeMachineModel.GetStockProduct(product.StockProductCode));

                var s = rog.StartGeneratingOrders();
            }
        }

        private static StockExchangeMachineModel _StockExchangeMachineModel = null;

    }
}
