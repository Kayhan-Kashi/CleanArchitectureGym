using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Infrastructure.Subscriptions.Persistance
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private readonly static List<Subscription> _subscriptions = new();

        public Task AddSubscriptionAsync(Subscription subscription)
        {
            _subscriptions.Add(subscription);
            return Task.CompletedTask;
        }

        public Task<Subscription?> GetByIdAsync(Guid subscriptionId)
        {
            var subscription = _subscriptions.FirstOrDefault(s => s.Id == subscriptionId);

            return Task.FromResult(subscription);
        }

    }
}