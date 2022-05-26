using BuyRequest.Data.Configuration;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BuyRequest.Data.Context
{
    public class BuyRequestDataContext : DataContext
    {
        public DbSet<Domain.Entities.BuyRequest> BuyRequests { get; set; }
        public DbSet<Domain.Entities.ProductRequest> ProductRequests { get; set; }
        public BuyRequestDataContext(DbContextOptions<BuyRequestDataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuyRequestConfiguration());
            modelBuilder.ApplyConfiguration(new ProductRequestConfiguration());
        }

    }
}
