using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Subscriptions.Persistance
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).ValueGeneratedNever();

            builder.Property("_adminId")
                .HasColumnName("AdminId"); // Although this field is readonly and it is not a property we can configute it to have it's own column

            builder.Property(s => s.SubscriptionType)
                .HasConversion(
                    convertToProviderExpression: subscriptionType => subscriptionType.Name, // on the way in
                    convertFromProviderExpression: value => SubscriptionType.FromName(value, false)  // on the way out
                );
        }

    }
}