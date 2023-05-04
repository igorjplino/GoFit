using System.Collections.Concurrent;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Repositories;

public class CachedTodoItemRepository : ITodoItemRepository
{
    private readonly ConcurrentDictionary<Guid, TodoItem?> _todoItems = new();

    private readonly ITodoItemRepository _todoItemRepository;
    
    public CachedTodoItemRepository(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public Guid Create(TodoItem todoItem)
    {
        return _todoItemRepository.Create(todoItem);
    }

    public TodoItem? GetTodoItem(Guid todoItemId)
    {
        if (_todoItems.TryGetValue(todoItemId, out TodoItem? todoItem))
        {
            return todoItem;
        }

        todoItem = _todoItemRepository.GetTodoItem(todoItemId);
        
        _todoItems.TryAdd(todoItemId, todoItem);

        return todoItem;
    }
}
