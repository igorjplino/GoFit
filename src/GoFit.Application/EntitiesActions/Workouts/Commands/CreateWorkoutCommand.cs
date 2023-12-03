using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Commands;

public record CreateWorkoutCommand(string Name, IEnumerable<WorkoutSet>? Sets) : IRequest<Result<Guid>>
{
    public string? Description { get; set; }
}

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Result<Guid>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<Result<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workoutPlan = new Workout
        {

        };

        return await _workoutRepository.CreateAsync(workoutPlan);
    }
}
