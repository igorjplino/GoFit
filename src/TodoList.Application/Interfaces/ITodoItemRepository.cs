using TodoList.Application.TodoItem.Dtos;

namespace TodoList.Application.Interfaces;

public interface ITodoItemRepository
{
    TodoItemDto? GetTodoItem(Guid todoItemId);
}
