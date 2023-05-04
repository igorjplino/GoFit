using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.TodoItems.Commands.Create;

public record CreateTodoItemCommand : IRequest<Guid>
{
    public string? Title { get; set; }
    public string? Note { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, Guid>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public CreateTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem
        {
            Note = request.Note,
            Title = request.Title
        };

        _todoItemRepository.Create(todoItem);

        return Task.FromResult(Guid.NewGuid());
    }
}
