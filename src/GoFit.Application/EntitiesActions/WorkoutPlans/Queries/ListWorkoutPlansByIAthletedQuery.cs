using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.Common;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Queries;

public record ListWorkoutPlansByIAthletedQuery(string AppUserId)
    : IRequest<Result<List<WorkoutPlanDto>>>
{ }

public class ListWorkoutPlansByIAthletedQueryHandler : IRequestHandler<ListWorkoutPlansByIAthletedQuery, Result<List<WorkoutPlanDto>>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IAthleteRepository _athleteRepository;

    public ListWorkoutPlansByIAthletedQueryHandler(IWorkoutPlanRepository workoutPlanRepository, IAthleteRepository athleteRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<List<WorkoutPlanDto>>> Handle(ListWorkoutPlansByIAthletedQuery request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        if (athlete is null)
            return Enumerable.Empty<WorkoutPlanDto>().ToList();

        List<WorkoutPlan> workoutPlans = await _workoutPlanRepository.ListPlansByAthleteIdAsync(athlete.Id);

        if (workoutPlans is null)
            return Enumerable.Empty<WorkoutPlanDto>().ToList();

        return workoutPlans.Select(ToDto).ToList();
    }

    private static WorkoutPlanDto ToDto(WorkoutPlan workoutPlan)
        => new()
        {
            Id = workoutPlan.Id,
            Title = workoutPlan.Title,
            Description= workoutPlan.Description,
            Workouts = workoutPlan.Workouts.Select(w => new WorkoutDto
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                Order = w.Order,
                WorkoutExercises = w.WorkoutExercises.Select(we => new WorkoutExerciseDto
                {
                    ExerciseId = we.ExerciseId,
                    ExerciseName = we.Exercise?.Name,
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
