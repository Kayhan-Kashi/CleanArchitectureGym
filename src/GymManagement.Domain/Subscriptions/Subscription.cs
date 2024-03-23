using Ardalis.SmartEnum;
using ErrorOr;
using GymManagement.Domain.Gyms;
using Throw;

namespace GymManagement.Domain.Subscriptions
{
    public class Subscription
    {
        private readonly List<Guid> _gymIds = new();
        private readonly int _maxGyms;
        public Guid AdminId { get; }

        public Guid Id { get; private set; }  // for populating this property via reflection we need private setter (EFCore will use it)
        public SubscriptionType SubscriptionType { get; private set; } // for populating this property via reflection we need private setter (EFCore will use it)

        public Subscription(
            SubscriptionType subscriptionType,
            Guid adminId,
            Guid? id = null)
        {
            SubscriptionType = subscriptionType;
            AdminId = adminId;
            Id = id ?? Guid.NewGuid();  // Public readonly properties without setter can be populated inside constructor ( public Guid Id {get; } )
        }

        private Subscription() { } // allow EFCore to do the migration and create an instance via reflection !!and we can say this is in contrast with persistance ignorance approach

        public ErrorOr<Success> AddGym(Gym gym)
        {
            _gymIds.Throw().IfContains(gym.Id);

            if (_gymIds.Count >= _maxGyms)
            {
                return SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows; // Domain level Error and validation level
            }

            _gymIds.Add(gym.Id);

            return Result.Success;
        }

        public int GetMaxGyms()
        {
            return SubscriptionType.Name switch
            {
                nameof(SubscriptionType.Free) => 1,
                nameof(SubscriptionType.Starter) => 1,
                nameof(SubscriptionType.Pro) => 3,
                _ => throw new InvalidOperationException()
            };
        }

        public int GetMaxRooms() => SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 1,
            nameof(SubscriptionType.Starter) => 3,
            nameof(SubscriptionType.Pro) => int.MaxValue,
            _ => throw new InvalidOperationException()
        };


        public int GetMaxDailySessions() => SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 4,
            nameof(SubscriptionType.Starter) => int.MaxValue,
            nameof(SubscriptionType.Pro) => int.MaxValue,
            _ => throw new InvalidOperationException()
        };

        public bool HasGym(Guid gymId)
        {
            return _gymIds.Contains(gymId);
        }

        public void RemoveGym(Guid gymId)
        {
            _gymIds.Throw().IfNotContains(gymId);

            _gymIds.Remove(gymId);
        }

    }
}