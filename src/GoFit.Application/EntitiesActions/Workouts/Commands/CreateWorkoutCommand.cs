using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Commands;

public record CreateWorkoutCommand(
    Guid WorkoutPlanId,
    string AppUserId,
    string Name,
    string? Description,
    IEnumerable<WorkoutExerciseDto> WorkoutExercises) : IRequest<Result<Guid>>
{ }

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Result<Guid>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IWorkoutRepository _workoutRepository;

    public CreateWorkoutCommandHandler(IWorkoutPlanRepository workoutPlanRepository, IWorkoutRepository workoutRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
        _workoutRepository = workoutRepository;
    }

    public async Task<Result<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var plan = await _workoutPlanRepository.GetPlanWithDetailsAsync(request.WorkoutPlanId);

        var workout = new Workout
        {
            WorkoutPlanId = request.WorkoutPlanId,
            Name = request.Name,
            Description = request.Description,
            Order = plan?.Workouts.Count() ?? 0,
            WorkoutExercises = ToModel(request.WorkoutExercises)
        };

        return await _workoutRepository.CreateAsync(workout);
    }

    private static List<WorkoutExercise> ToModel(IEnumerable<WorkoutExerciseDto> workoutExercises)
        => workoutExercises.Select(we => new WorkoutExercise
        {
            ExerciseId = we.ExerciseId,
            Order = we.Order,
            Sets = we.Sets.Select(ws => new WorkoutSet
            {
                WarmUp = ws.WarmUp,
                UntilFailure = ws.UntilFailure,
                MinRepetitions = ws.MinRepetitions,
                MaxRepetitions = ws.MaxRepetitions,
                ResetTime = ws.ResetTime,
                Weight = ws.Weight,
                Order = ws.Order
            }).ToList()
        }).ToList();
}
