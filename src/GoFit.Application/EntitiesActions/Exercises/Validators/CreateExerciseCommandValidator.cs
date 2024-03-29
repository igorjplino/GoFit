﻿using FluentValidation;
using GoFit.Application.EntitiesActions.Exercises.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.Exercises.Validators;
public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    private readonly IExerciseRepository _exerciseRepository;

    public CreateExerciseCommandValidator(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .MustAsync(IsUniqueName).WithMessage("The exercise name '{PropertyValue}' already exists");

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .MinimumLength(3)
                .MaximumLength(300);
        });
    }

    private async Task<bool> IsUniqueName(string name, CancellationToken ct)
    {
        var exercise = await _exerciseRepository.GetByNameAsync(name);

        return exercise is null;
    }
}
