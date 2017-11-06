using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StockExchangeMachine.Web.Models
{
    public class TheContext : DbContext
    {
        public TheContext (DbContextOptions<TheContext> options)
            : base(options)
        {
        }

        public DbSet<StockExchangeMachine.Web.Models.StockSaleViewModel> StockSaleViewModel { get; set; }
    }
}
