﻿using FluentValidation;

namespace GoFit.Application.TodoItems.Commands.Create;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
	public CreateTodoItemCommandValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty();
	}
}
