using FluentValidation;

namespace GoFit.Application.Workouts.Commands;
internal class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutCommandValidator()
    {
    }
}
