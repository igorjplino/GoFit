using MediatR;

namespace TodoList.Application.TodoItem.Commands.Create;

public record CreateTodoItemCommand : IRequest<TempResponse>
{
    public string? Title { get; set; }
    public string Note { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TempResponse>
{
    public Task<TempResponse> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TempResponse { Temp = "Created" });
    }
}
