using ErrorOr;
using MediatR;

namespace GymManagement.Application.Gyms.Command.DeleteGym
{
    public record DeleteGymCommand(Guid SubscriptionId, Guid GymId) : IRequest<ErrorOr<Deleted>>;
}