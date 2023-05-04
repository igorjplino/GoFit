using MediatR;
using Microsoft.AspNetCore.Mvc;
using GoFit.Application.TodoItems.Commands.Create;
using GoFit.Application.TodoItems.Dtos;
using GoFit.Application.TodoItems.Queries;

namespace GoFit.Api.Controllers;

public class TodoItemsController : ApiControllerBase
{
    public TodoItemsController(IMediator mediator) : base(mediator)
    { }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetById([FromRoute] Guid id)
    {
        TodoItemDto? result = await Mediator.Send(new GetTodoItemByIdQuery { Id = id });

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateTodoItemCommand command)
    {
        Guid result = await Mediator.Send(command);
        
        return Ok(result);
    }
}
