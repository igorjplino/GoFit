using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Queries;

public record GetWorkoutPlanDtoByIdQuery : IRequest<WorkoutPlanDto?>
{
    public Guid Id { get; set; }
}

public class GetWorkoutPlanDtoByIdQueryHandler : IRequestHandler<GetWorkoutPlanDtoByIdQuery, WorkoutPlanDto?>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;

    public GetWorkoutPlanDtoByIdQueryHandler(IWorkoutPlanRepository workoutPlanRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
    }

    public async Task<WorkoutPlanDto?> Handle(GetWorkoutPlanDtoByIdQuery request, CancellationToken cancellationToken)
    {
        WorkoutPlan? workoutPlan = await _workoutPlanRepository.GetAsync(request.Id);

        if (workoutPlan is null)
            return null;

        var workoutPlanDto = new WorkoutPlanDto
        {
            Title = workoutPlan.Title
        };

        return workoutPlanDto;
    }
}
