using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Commands;

public record CreateWorkoutCommand(
    Guid WorkoutPlanId,
    string Name,
    string? Description,
    IEnumerable<WorkoutSet>? Sets) : IRequest<Result<Guid>>
{ }

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Result<Guid>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<Result<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = new Workout
        {
            WorkoutPlanId = request.WorkoutPlanId,
            Name = request.Name,
            Description = request.Description
        };

        return await _workoutRepository.CreateAsync(workout);
    }
}
