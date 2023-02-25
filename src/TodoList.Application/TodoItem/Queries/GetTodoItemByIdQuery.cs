using MediatR;
using TodoList.Application.Interfaces;
using TodoList.Application.TodoItem.Dtos;

namespace TodoList.Application.TodoItem.Queries;

public record GetTodoItemByIdQuery : IRequest<TodoItemDto?>
{
    public Guid Id { get; set; }
}

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItemDto?>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public GetTodoItemByIdQueryHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public Task<TodoItemDto?> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        TodoItemDto? todoItem = _todoItemRepository.GetTodoItem(request.Id);

        return Task.FromResult(todoItem);
    }
}
