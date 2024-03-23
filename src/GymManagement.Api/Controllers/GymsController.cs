using ErrorOr;
using GymManagement.Application.Gyms.Command.AddTrainer;
using GymManagement.Application.Gyms.Command.CreateGym;
using GymManagement.Application.Gyms.Command.DeleteGym;
using GymManagement.Application.Gyms.Queries.GetGym;
using GymManagement.Application.Gyms.Queries.ListGyms;
using GymManagement.Contracts.Gyms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Error = ErrorOr.Error;

namespace GymManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GymsController : ApiController
    {
        private readonly IMediator _mediator;

        public GymsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGym(
            CreateGymRequest request,
            Guid subscriptionId)
        {
            var command = new CreateGymCommand(request.Name, subscriptionId);

            var createGymResult = await _mediator.Send(command);

            return createGymResult.Match(
                gym => CreatedAtAction(
                    nameof(GetGym),
                    new { subscriptionId, GymId = gym.Id },
                    new GymResponse(gym.Id, gym.Name)),
                errors => Problem(errors));
        }

        [HttpDelete("gymId:guid")]
        public async Task<IActionResult> DeleteGym(Guid subscriptionId, Guid gymId)
        {
            var command = new DeleteGymCommand(subscriptionId, gymId);

            var deleteGymResult = await _mediator.Send(command);

            return deleteGymResult.Match<IActionResult>(
                _ => NoContent(),
                Problem
            );
        }


        [HttpGet("{gymId:guid}")]
        public async Task<IActionResult> GetGym(Guid subscriptionId, Guid gymId)
        {
            var command = new GetGymQuery(subscriptionId, gymId);
            var getGymResult = await _mediator.Send(command);

            return getGymResult.Match(
                gym => Ok(new GymResponse(gym.Id, gym.Name)),
                _ => Problem());
        }

        [HttpGet]
        public async Task<IActionResult> ListGyms(Guid subscriptionId)
        {
            var query = new ListGymsQuery(subscriptionId);
            var listGymsResult = _mediator.Send(query);

            return await listGymsResult.Match(
                gyms => Ok(gyms.ConvertAll(gym => new GymResponse(gym.Id, gym.Name))),
                _ => Problem());
        }

        [HttpPost("{gymId:guid}/trainers")]
        public async Task<IActionResult> AddTrainer(AddTrainerRequest request, Guid subscriptionId, Guid gymId)
        {
            var command = new AddTrainerCommand(subscriptionId, gymId, request.TrainerId);
            var addTrainerResult = await _mediator.Send(command);

            return addTrainerResult.Match<IActionResult>(
                _ => Ok(),
                _ => Problem()
            );
        }


    }
}