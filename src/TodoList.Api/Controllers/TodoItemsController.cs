using Microsoft.AspNetCore.Mvc;
using TodoList.Application;
using TodoList.Application.TodoItem.Queries;

namespace TodoList.Api.Controllers;

public class TodoItemsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<TempResponse>> GetById([FromRoute] Guid id)
    {
        TempResponse result = await Mediator.Send(new GetTodoItemByIdQuery { Id = id });

        return Ok(result);
    }
}
