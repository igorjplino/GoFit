using MediatR;
using TodoList.Application.Interfaces;
using TodoList.Application.TodoItems.Dtos;
using TodoList.Domain.Entities;

namespace TodoList.Application.TodoItems.Queries;

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
        TodoItem? todoItem = _todoItemRepository.GetTodoItem(request.Id);

        if (todoItem is null)
            return Task.FromResult(null as TodoItemDto);

        return Task.FromResult<TodoItemDto?>(new TodoItemDto { Title = todoItem.Title, Note = todoItem.Note });
    }
}
