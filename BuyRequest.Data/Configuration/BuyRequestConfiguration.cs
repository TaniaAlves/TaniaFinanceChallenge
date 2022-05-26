using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuyRequest.Data.Configuration
{
    public class BuyRequestConfiguration : IEntityTypeConfiguration<Domain.Entities.BuyRequest>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.BuyRequest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.StreetDescription);
            builder.Property(x => x.Price);
            builder.Property(x => x.CostValue);
            builder.Property(x => x.City);
            builder.Property(x => x.ClientDescription);
            builder.Property(x => x.ClientEmail);
            builder.Property(x => x.ClientId);
            builder.Property(x => x.ClientPhone);
            builder.Property(x => x.Code);
            builder.Property(x => x.Complement);
            builder.Property(x => x.Date);
            builder.Property(x => x.TotalValue);
            builder.Property(x => x.DeliveryDate);
            builder.Property(x => x.DiscountValue);

            builder.HasMany(x => x.Products).WithOne(x => x.BuyRequest).HasForeignKey(x => x.BuyRequestId); /*/.HasConstraintName("Fk_BuyRequests") ;*/
        }
    }
}
