using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeMachine.Web.Models
{
    public class StockSaleViewModel
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}
