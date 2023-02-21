using FluentValidation;

namespace TodoList.Application.TodoItem.Commands.Create;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
	public CreateTodoItemCommandValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty();
	}
}
