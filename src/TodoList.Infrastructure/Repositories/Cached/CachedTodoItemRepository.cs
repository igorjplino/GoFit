using System.Collections.Concurrent;
using TodoList.Application.Interfaces;
using TodoList.Application.TodoItem.Dtos;

namespace TodoList.Infrastructure.Repositories;

public class CachedTodoItemRepository : ITodoItemRepository
{
    private readonly ConcurrentDictionary<Guid, TodoItemDto?> _todoItems = new();

    private readonly ITodoItemRepository _todoItemRepository;
    
    public CachedTodoItemRepository(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public TodoItemDto? GetTodoItem(Guid todoItemId)
    {
        if (_todoItems.TryGetValue(todoItemId, out TodoItemDto? todoItem))
        {
            return todoItem;
        }

        todoItem = _todoItemRepository.GetTodoItem(todoItemId);
        
        _todoItems.TryAdd(todoItemId, todoItem);

        return todoItem;
    }
}
