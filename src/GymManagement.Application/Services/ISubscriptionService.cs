namespace GymManagement.Application.Services
{
    public interface ISubscriptionWriteService
    {
        Guid CreateSubscriptions(string subscriptionType, Guid adminId);
    }
}