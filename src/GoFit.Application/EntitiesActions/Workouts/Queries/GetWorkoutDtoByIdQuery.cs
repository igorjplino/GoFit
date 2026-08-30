using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;
using WorkoutDto = GoFit.Application.EntitiesActions.Workouts.Dtos.WorkoutDto;

namespace GoFit.Application.EntitiesActions.Workouts.Queries;

public record GetWorkoutDtoByIdQuery(Guid Id, string AppUserId = "")
    : IRequest<Result<WorkoutDto?>>
{ }

public class GetWorkoutDtoByIdQueryHandler : IRequestHandler<GetWorkoutDtoByIdQuery, Result<WorkoutDto?>>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IAthleteRepository _athleteRepository;

    public GetWorkoutDtoByIdQueryHandler(IWorkoutRepository workoutRepository, IAthleteRepository athleteRepository)
    {
        _workoutRepository = workoutRepository;
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<WorkoutDto?>> Handle(GetWorkoutDtoByIdQuery request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        if (athlete is null)
            return default;

        Workout? workout = await _workoutRepository.GetWithDetailsAsync(request.Id);

        if (workout is null)
            return default;

        if (workout.WorkoutPlan.AthleteId != athlete.Id)
            return default;

        return ToDto(workout);
    }

    private static WorkoutDto ToDto(Workout workout)
        => new()
        {
            Id = workout.Id,
            WorkoutPlanId = workout.WorkoutPlanId,
            Name = workout.Name,
            Description = workout.Description,
            Order = workout.Order,
            WorkoutExercises = workout.WorkoutExercises.Select(we => new WorkoutExerciseDto
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
        };
}
