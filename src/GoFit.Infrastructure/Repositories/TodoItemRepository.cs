using System.Collections.Concurrent;
using TodoList.Application.Interfaces;
using TodoList.Application.TodoItems.Dtos;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly ConcurrentDictionary<Guid, TodoItem> _todoItems = new();

    public TodoItemRepository()
    {
        _todoItems.TryAdd(new Guid("b374dd0d-fc89-40d8-b34b-5fc50839a005"), new TodoItem { Title = "Note 1", Note = "fisrt thing todo" });
        _todoItems.TryAdd(new Guid("00d4b523-e974-4e85-894e-20b1d4a78fb7"), new TodoItem { Title = "Note 2", Note = "second thing todo" });
        _todoItems.TryAdd(new Guid("1ed13a4b-b2c5-411e-b0d3-448bb8b49831"), new TodoItem { Title = "Note 3", Note = "third thing todo" });
    }

    public Guid Create(TodoItem todoItem)
    {
        var id = Guid.NewGuid();

        _todoItems.TryAdd(id, todoItem);

        return id;
    }

    public TodoItem? GetTodoItem(Guid todoItemId)
    {
        if (_todoItems.TryGetValue(todoItemId, out TodoItem? todoItem))
        {
            return todoItem;
        }

        return null;
    }
}
