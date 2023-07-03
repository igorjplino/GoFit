using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Commands;
public record CreateWorkoutCommand : IRequest<ValidatorResponse<Guid>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<WorkoutSet>? Sets { get; set; }
}

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, ValidatorResponse<Guid>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<ValidatorResponse<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workoutPlan = new Workout
        {

        };

        Guid id = await _workoutRepository.CreateAsync(workoutPlan);

        return ValidatorResponse<Guid>.Success(id);
    }
}
