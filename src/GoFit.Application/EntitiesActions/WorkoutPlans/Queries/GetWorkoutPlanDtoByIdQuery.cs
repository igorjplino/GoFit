using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.Common;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Queries;

public record GetWorkoutPlanDtoByIdQuery : IRequest<ValidatorResponse<WorkoutPlanDto?>>
{
    public Guid Id { get; set; }
}

public class GetWorkoutPlanDtoByIdQueryHandler : IRequestHandler<GetWorkoutPlanDtoByIdQuery, ValidatorResponse<WorkoutPlanDto?>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;

    public GetWorkoutPlanDtoByIdQueryHandler(IWorkoutPlanRepository workoutPlanRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
    }

    public async Task<ValidatorResponse<WorkoutPlanDto?>> Handle(GetWorkoutPlanDtoByIdQuery request, CancellationToken cancellationToken)
    {
        WorkoutPlan? workoutPlan = await _workoutPlanRepository.GetAsync(request.Id);

        if (workoutPlan is null)
            return ValidatorResponse<WorkoutPlanDto?>.Success(null);

        var workoutPlanDto = new WorkoutPlanDto
        {
            Title = workoutPlan.Title
        };

        return ValidatorResponse<WorkoutPlanDto?>.Success(workoutPlanDto);
    }
}
