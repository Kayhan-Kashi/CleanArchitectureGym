using Ardalis.SmartEnum;

namespace GymManagement.Domain.Subscriptions
{
    public class Subscription
    {
        private readonly Guid _adminId;
        public Guid Id { get; private set; }  // for populating this property via reflection we need private setter (EFCore will use it)
        public SubscriptionType SubscriptionType { get; private set; } // for populating this property via reflection we need private setter (EFCore will use it)

        public Subscription(
            SubscriptionType subscriptionType,
            Guid adminId,
            Guid? id = null)
        {
            SubscriptionType = subscriptionType;
            _adminId = adminId;
            Id = id ?? Guid.NewGuid();  // Public readonly properties without setter can be populated inside constructor ( public Guid Id {get; } )
        }

        private Subscription() { } // allow EFCore to do the migration and create an instance via reflection !!and we can say this is in contrast with persistance ignorance approach

    }
}