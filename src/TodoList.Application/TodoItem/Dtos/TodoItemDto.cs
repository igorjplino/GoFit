namespace TodoList.Application.TodoItem.Dtos;

public record TodoItemDto
{
    public string? Title { get; set; }
    public string? Note { get; set; }
}
