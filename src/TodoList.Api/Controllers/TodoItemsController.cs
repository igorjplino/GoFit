using Microsoft.AspNetCore.Mvc;
using TodoList.Application.TodoItems.Commands.Create;
using TodoList.Application.TodoItems.Dtos;
using TodoList.Application.TodoItems.Queries;

namespace TodoList.Api.Controllers;

public class TodoItemsController : ApiControllerBase
{
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
