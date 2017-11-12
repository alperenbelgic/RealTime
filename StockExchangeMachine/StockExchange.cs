using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace StockExchangeMachine
{
    public class StockExchangeMachineModel
    {
        private List<StockProduct> StockProducts = new List<StockProduct>();

        public void AddStockProduct(StockProduct stockProduct)
        {
            this.StockProducts.Add(stockProduct);
        }

        public StockProduct GetStockProduct(string stockProductCode)
        {
            return StockProducts.FirstOrDefault(sp => sp.StockProductCode == stockProductCode);
        }
    }
}
