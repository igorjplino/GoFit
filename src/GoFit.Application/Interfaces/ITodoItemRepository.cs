using TodoList.Application.TodoItems.Dtos;
using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces;

public interface ITodoItemRepository
{
    Guid Create(TodoItem todoItem);
    TodoItem? GetTodoItem(Guid todoItemId);
}
