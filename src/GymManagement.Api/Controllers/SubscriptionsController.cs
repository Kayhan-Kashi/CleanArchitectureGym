using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Commands.DeleteSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using DomainSubscriptiontype = GymManagement.Domain.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ApiController
    {
        private readonly IMediator _mediator;

        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
        {
            if (!DomainSubscriptiontype.TryFromName(
                request.SubscriptionType.ToString(),
                out var subscriptionType))
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: "Invalid subscription type");
            }

            var command = new CreateSubscriptionCommand(
                subscriptionType,
                request.AdminId);

            var createSubscriptionResult = await _mediator.Send(command); // we use ErrorOr package that do the Result pattern

            return createSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubscriptionType)),
                error => Problem()
            );
        }

        [HttpGet("{subscriptionId:guid}")]
        public async Task<IActionResult> GetSubscription(Guid subscriptionId)
        {
            var query = new GetSubscriptionQuery(subscriptionId);

            var getSubscriptionResult = await _mediator.Send(query);

            return getSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(
                    subscription.Id,
                    Enum.Parse<SubscriptionType>(subscription.SubscriptionType.Name))),
                error => Problem()
            );

        }

        [HttpDelete("{subscriptionId:guid}")]
        public async Task<IActionResult> DeleteSubscription(Guid subscriptionId)
        {
            var command = new DeleteSubscriptionCommand(subscriptionId);

            var createSubscriptionResult = await _mediator.Send(command);

            return createSubscriptionResult.Match<IActionResult>(
                _ => NoContent(),
                _ => Problem());
        }

        // private static SubscriptionType ToDto(DomainSubscriptionType subscriptionType)
        // {
        //     return subscriptionType.Name switch
        //     {
        //         nameof(DomainSubscriptionType.Free) => SubscriptionType.Free,
        //         nameof(DomainSubscriptionType.Starter) => SubscriptionType.Starter,
        //         nameof(DomainSubscriptionType.Pro) => SubscriptionType.Pro,
        //         _ => throw new InvalidOperationException(),
        //     };
        // }
    }
}