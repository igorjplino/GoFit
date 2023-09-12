using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Commands;

public record CreateWorkoutPlanCommand : IRequest<Result<Guid>>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<WorkoutExerciseDto> Exercises { get; set; }
}

public class CreateWorkoutPlanCommandHandler : IRequestHandler<CreateWorkoutPlanCommand, Result<Guid>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;

    public CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
    }

    public async Task<Result<Guid>> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var workoutPlan = ToModel(request);

        return await _workoutPlanRepository.CreateAsync(workoutPlan);
    }

    private static WorkoutPlan ToModel(CreateWorkoutPlanCommand request)
        => new()
        {
            Title = request.Title,
            Description = request.Description,
            Workouts = request.Exercises?.Select(w => new Workout
            {
                ExerciseId = w.ExerciseId,
                Order = w.Order,
                Sets = w.Sets.Select(ws => new WorkoutSet
                {
                    WarmUp = ws.WarmUp,
                    UntilFailure = ws.UntilFailure,
                    MinRepetitions = ws.MinRepetitions,
                    MaxRepetitions = ws.MaxRepetitions,
                    ResetTime = ws.ResetTime,
                    Order = ws.Order
                }).ToList()
            }).ToList()
        };
}
