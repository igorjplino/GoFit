using GoFit.Application.TodoItems.Dtos;
using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;

public interface ITodoItemRepository
{
    Guid Create(TodoItem todoItem);
    TodoItem? GetTodoItem(Guid todoItemId);
}
