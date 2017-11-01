using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RealTime.Models
{
    public class RealTimeContext : DbContext
    {
        public RealTimeContext (DbContextOptions<RealTimeContext> options)
            : base(options)
        {
        }

        public DbSet<RealTime.Models.MyModel> MyModel { get; set; }
    }
}
