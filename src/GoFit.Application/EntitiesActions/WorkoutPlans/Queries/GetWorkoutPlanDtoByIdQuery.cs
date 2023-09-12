using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.Common;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Queries;

public record GetWorkoutPlanDtoByIdQuery : IRequest<Result<WorkoutPlanDto?>>
{
    public Guid Id { get; set; }
}

public class GetWorkoutPlanDtoByIdQueryHandler : IRequestHandler<GetWorkoutPlanDtoByIdQuery, Result<WorkoutPlanDto?>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;

    public GetWorkoutPlanDtoByIdQueryHandler(IWorkoutPlanRepository workoutPlanRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
    }

    public async Task<Result<WorkoutPlanDto?>> Handle(GetWorkoutPlanDtoByIdQuery request, CancellationToken cancellationToken)
    {
        WorkoutPlan? workoutPlan = await _workoutPlanRepository.GetPlanWithDetailsAsync(request.Id);

        if (workoutPlan is null)
            return default;

        return ToDto(workoutPlan);
    }

    private WorkoutPlanDto ToDto(WorkoutPlan workoutPlan)
        => new()
        {
            Title = workoutPlan.Title,
            Description= workoutPlan.Description,
            Workouts = workoutPlan.Workouts.Select(w => new WorkoutExerciseDto
            {
                ExerciseId = w.ExerciseId,
                ExerciseName = w.Exercise.Name,
                ExerciseDescription = w.Exercise.Description,
                Order = w.Order,
                Sets = w.Sets.Select(ws => new WorkoutExerciseSetDto
                {
                    WarmUp = ws.WarmUp,
                    UntilFailure = ws.UntilFailure,
                    MinRepetitions = ws.MinRepetitions,
                    MaxRepetitions = ws.MaxRepetitions,
                    ResetTime = ws.ResetTime,
                    Weight = ws.Weight,
                    Order = ws.Order                    
                })
            })
        };
}
