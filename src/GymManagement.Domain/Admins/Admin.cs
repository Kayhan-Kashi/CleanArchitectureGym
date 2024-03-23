using GymManagement.Domain.Subscriptions;
using Throw;

namespace GymManagement.Domain.Admins
{
    public class Admin
    {
        public Guid Id { get; set; }
        public Guid? SubscriptionId { get; set; }
        public Guid UserId { get; set; }

        public Admin(Guid userId, Guid? subscriptionId = null, Guid? id = null)
        {
            UserId = userId;
            SubscriptionId = subscriptionId;
            Id = id ?? Guid.NewGuid();
        }

        private Admin() { }

        public void SetSubscription(Subscription subscription)
        {
            SubscriptionId.HasValue.Throw().IfTrue();
            SubscriptionId = subscription.Id;
        }

        public void DeleteSubscription(Guid subscriptionId)
        {
            SubscriptionId.ThrowIfNull().IfNotEquals(subscriptionId);

            SubscriptionId = null;
        }
    }
}