using GymManagement.Application.Services;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
        {
            var command = new CreateSubscriptionCommand(
                request.subscriptionType.ToString(),
                request.adminId);

            var createSubscriptionResult = await _mediator.Send(command); // we use ErrorOr package that do the Result pattern
            if (createSubscriptionResult.IsError)
            {
                return Problem();  // return 500
            }


            var response = new SubscriptionResponse(
                createSubscriptionResult.Value,  // returns underline Guid value
                request.subscriptionType);

            return Ok(response);
        }
    }
}