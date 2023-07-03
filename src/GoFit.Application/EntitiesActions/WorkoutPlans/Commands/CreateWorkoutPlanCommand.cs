using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.Common;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Commands;

public record CreateWorkoutPlanCommand : IRequest<ValidatorResponse<Guid>>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<Guid>? Workouts { get; set; }
}

public class CreateWorkoutPlanCommandHandler : IRequestHandler<CreateWorkoutPlanCommand, ValidatorResponse<Guid>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;

    public CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
    }

    public async Task<ValidatorResponse<Guid>> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var workoutPlan = new WorkoutPlan
        {
            Title = request.Title,
            Description = request.Description
        };

        Guid id = await _workoutPlanRepository.CreateAsync(workoutPlan);

        return ValidatorResponse<Guid>.Success(id);
    }
}
