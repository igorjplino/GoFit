using MediatR;
using Microsoft.AspNetCore.Mvc;
using GoFit.Application.WorkoutPlans.Commands.Create;
using GoFit.Application.WorkoutPlans.Queries;
using GoFit.Application.WorkoutPlans.Dtos;

namespace GoFit.Api.Controllers;

public class WorkoutPlanController : ApiControllerBase
{
    public WorkoutPlanController(IMediator mediator) : base(mediator)
    { }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutPlanDto>> GetById([FromRoute] Guid id)
    {
        WorkoutPlanDto? result = await Mediator.Send(new GetWorkoutPlanDtoByIdQuery { Id = id });

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateWorkoutPlanCommand command)
    {
        Guid result = await Mediator.Send(command);
        
        return Ok(result);
    }
}
