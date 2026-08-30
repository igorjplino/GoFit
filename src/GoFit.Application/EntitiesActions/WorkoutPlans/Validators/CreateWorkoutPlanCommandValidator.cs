using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Validators;

public class CreateWorkoutPlanCommandValidator : AbstractValidator<CreateWorkoutPlanCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public CreateWorkoutPlanCommandValidator(
        IExerciseRepository exerciseRepository,
        IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.AppUserId)
            .NotEmpty()
            .MustAsync(HaveLinkedAthlete).WithMessage("No athlete is linked to the current account.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .MinimumLength(3)
                .MaximumLength(300);
        });

        RuleFor(x => x.Workouts)
            .NotEmpty();

        RuleForEach(x => x.Workouts)
            .NotEmpty()
            .SetValidator(new WorkoutDtoValidator(exerciseRepository));
    }

    private async Task<bool> HaveLinkedAthlete(string appUserId, CancellationToken ct)
        => await _athleteRepository.GetByAppUserIdAsync(appUserId) is not null;
}
