
namespace GymManagement.Application.Services
{
    public class SubscriptionWriteService : ISubscriptionWriteService
    {
        public Guid CreateSubscriptions(string subscriptionType, Guid adminId)
        {
            return Guid.NewGuid();
        }

    }
}