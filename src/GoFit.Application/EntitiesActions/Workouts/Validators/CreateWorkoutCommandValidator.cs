using FluentValidation;
using GoFit.Application.EntitiesActions.Workouts.Commands;

namespace GoFit.Application.EntitiesActions.Workouts.Validators;
internal class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutCommandValidator()
    {
    }
}
