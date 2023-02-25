using MediatR;

namespace TodoList.Application.TodoItem.Commands.Create;

public record CreateTodoItemCommand : IRequest<Guid>
{
    public string? Title { get; set; }
    public string? Note { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, Guid>
{
    public Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}
