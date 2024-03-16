using System.Data.Common;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common;

namespace GymManagement.Infrastructure.Subscriptions.Persistance
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        //private readonly static List<Subscription> _subscriptions = new();
        private readonly GymManagementDbContext _dbContext;

        public SubscriptionsRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            await _dbContext.Subscriptions.AddAsync(subscription);
        }

        public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
        {
            return await _dbContext.Subscriptions.FindAsync(subscriptionId);
        }

    }
}