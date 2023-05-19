using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.WorkoutPlans.Dtos;

namespace GoFit.Application.WorkoutPlans.Queries;

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

    public Task<WorkoutPlanDto?> Handle(GetWorkoutPlanDtoByIdQuery request, CancellationToken cancellationToken)
    {
        WorkoutPlan? workoutPlan = _workoutPlanRepository.Get(request.Id);

        if (workoutPlan is null)
            return Task.FromResult(null as WorkoutPlanDto);

        var dto = new WorkoutPlanDto
        {
            Title = workoutPlan.Title
        };

        return Task.FromResult<WorkoutPlanDto?>(dto);
    }
}
