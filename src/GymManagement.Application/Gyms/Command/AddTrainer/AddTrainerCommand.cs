using ErrorOr;
using MediatR;

namespace GymManagement.Application.Gyms.Command.AddTrainer
{
    public record AddTrainerCommand(Guid SubscriptionId, Guid GymId, Guid TrainerId) : IRequest<ErrorOr<Success>>;
}