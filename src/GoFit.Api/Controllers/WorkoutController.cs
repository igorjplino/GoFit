using MediatR;
using Microsoft.AspNetCore.Mvc;
using GoFit.Application.Workouts.Queries;
using GoFit.Application.Workouts.Dtos;
using GoFit.Application.Workouts.Commands;

namespace GoFit.Api.Controllers;

public class WorkoutController : ApiControllerBase
{
    public WorkoutController(IMediator mediator) : base(mediator)
    { }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutDto>> GetById([FromRoute] Guid id)
    {
        WorkoutDto? result = await Mediator.Send(new GetWorkoutDtoByIdQuery { Id = id });

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateWorkoutCommand command)
    {
        Guid result = await Mediator.Send(command);
        
        return Ok(result);
    }
}
