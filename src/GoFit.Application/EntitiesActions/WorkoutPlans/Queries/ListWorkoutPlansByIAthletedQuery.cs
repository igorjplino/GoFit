using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.Common;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Queries;

public record ListWorkoutPlansByIAthletedQuery(Guid AthleteId)
    : IRequest<Result<List<WorkoutPlanDto>>>
{ }

public class ListWorkoutPlansByIAthletedQueryHandler : IRequestHandler<ListWorkoutPlansByIAthletedQuery, Result<List<WorkoutPlanDto>>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;

    public ListWorkoutPlansByIAthletedQueryHandler(IWorkoutPlanRepository workoutPlanRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
    }

    public async Task<Result<List<WorkoutPlanDto>>> Handle(ListWorkoutPlansByIAthletedQuery request, CancellationToken cancellationToken)
    {
        List<WorkoutPlan> workoutPlans = await _workoutPlanRepository.ListPlansByAthleteIdAsync(request.AthleteId);

        if (workoutPlans is null)
            return Enumerable.Empty<WorkoutPlanDto>().ToList();

        return workoutPlans.Select(ToDto).ToList();
    }

    private static WorkoutPlanDto ToDto(WorkoutPlan workoutPlan)
        => new()
        {
            Title = workoutPlan.Title,
            Description= workoutPlan.Description,
            Workouts = workoutPlan.Workouts.Select(w => new WorkoutDto
            {
                Name = w.Name,
                Description = w.Description,
                Order = w.Order,
                WorkoutExercises = w.WorkoutExercises.Select(we => new WorkoutExerciseDto
                {
                    ExerciseId = we.ExerciseId,
                    Order = we.Order,
                    Sets = we.Sets.Select(ws => new WorkoutExerciseSetDto
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
            })
        };
}
