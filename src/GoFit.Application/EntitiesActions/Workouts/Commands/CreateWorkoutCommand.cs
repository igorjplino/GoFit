using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Commands;
public record CreateWorkoutCommand : IRequest<Guid>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<WorkoutSet>? Sets { get; set; }
}

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Guid>
{
    private readonly IWorkoutRepository _workoutRepository;

    public CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<Guid> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workoutPlan = new Workout
        {

        };

        return await _workoutRepository.CreateAsync(workoutPlan);
    }
}
