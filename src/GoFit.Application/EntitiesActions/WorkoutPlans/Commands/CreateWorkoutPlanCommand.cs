using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Domain;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Commands;

public record CreateWorkoutPlanCommand(
    string Title,
    string? Description,
    IEnumerable<WorkoutDto> Workouts)
    : IRequest<Result<Guid>>
{ }

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
            Workouts = request.Workouts.Select(w => new Workout
            {
                Name = w.Name,
                Description = w.Description,
                Order = w.Order,
                WorkoutExercises = w.WorkoutExercises.Select(we => new WorkoutExercise
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
                }).ToList()
            }).ToList()
        };
}
