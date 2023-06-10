using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Commands;

public record CreateWorkoutPlanCommand : IRequest<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<Guid>? Workouts { get; set; }
}

public class CreateWorkoutPlanCommandHandler : IRequestHandler<CreateWorkoutPlanCommand, Guid>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;

    public CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
    }

    public async Task<Guid> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var workoutPlan = new WorkoutPlan
        {
            Title = request.Title,
            Description = request.Description
        };

        return await _workoutPlanRepository.CreateAsync(workoutPlan);
    }
}
