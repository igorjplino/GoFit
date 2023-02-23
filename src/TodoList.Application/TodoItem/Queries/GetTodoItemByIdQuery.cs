using MediatR;
using TodoList.Application.Interfaces;
using TodoList.Application.TodoItem.Dtos;

namespace TodoList.Application.TodoItem.Queries;

public record GetTodoItemByIdQuery : IRequest<TempResponse>
{
    public Guid Id { get; set; }
}

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TempResponse>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public GetTodoItemByIdQueryHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public Task<TempResponse> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        TodoItemDto? todoItem = _todoItemRepository.GetTodoItem(request.Id);

        return Task.FromResult(new TempResponse { Temp = "Temporary response", ResultObj = todoItem });
    }
}
