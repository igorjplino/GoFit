using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutPlans.Validators;
using GoFit.Application.EntitiesActions.Workouts.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.Workouts.Validators;
internal class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IAthleteRepository _athleteRepository;

    public CreateWorkoutCommandValidator(
        IWorkoutPlanRepository workoutPlanRepository,
        IAthleteRepository athleteRepository,
        IExerciseRepository exerciseRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
        _athleteRepository = athleteRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.AppUserId)
            .NotEmpty();

        RuleFor(x => x)
            .MustAsync(BelongToCallingAthlete).WithMessage("Workout plan not found.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .MinimumLength(3)
                .MaximumLength(300);
        });

        RuleFor(x => x.WorkoutExercises)
            .NotEmpty();

        RuleForEach(x => x.WorkoutExercises)
            .SetValidator(new WorkoutExerciseDtoValidator(exerciseRepository));
    }

    private async Task<bool> BelongToCallingAthlete(CreateWorkoutCommand command, CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(command.AppUserId);

        var plan = await _workoutPlanRepository.GetAsync(command.WorkoutPlanId);

        return plan is not null && athlete is not null && plan.AthleteId == athlete.Id;
    }
}
