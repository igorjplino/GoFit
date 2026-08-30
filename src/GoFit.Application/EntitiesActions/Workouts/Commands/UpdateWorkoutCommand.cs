using MediatR;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

namespace GoFit.Application.EntitiesActions.Workouts.Commands;

public record UpdateWorkoutCommand(
    Guid WorkoutId,
    string AppUserId,
    string Name,
    string? Description,
    IEnumerable<WorkoutExerciseDto> WorkoutExercises)
    : IRequest<Result<Guid?>>
{ }

public class UpdateWorkoutCommandHandler : IRequestHandler<UpdateWorkoutCommand, Result<Guid?>>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IAthleteRepository _athleteRepository;

    public UpdateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IAthleteRepository athleteRepository)
    {
        _workoutRepository = workoutRepository;
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<Guid?>> Handle(UpdateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        if (athlete is null)
            return default;

        Workout? workout = await _workoutRepository.GetWithDetailsAsync(request.WorkoutId);

        if (workout is null)
            return default;

        if (workout.WorkoutPlan.AthleteId != athlete.Id)
            return default;

        var exercises = ToModel(request.WorkoutExercises, workout.Id);

        await _workoutRepository.UpdateWorkoutAsync(workout.Id, request.Name, request.Description, exercises);

        return workout.Id;
    }

    private static IEnumerable<WorkoutExercise> ToModel(IEnumerable<WorkoutExerciseDto> workoutExercises, Guid workoutId)
        => workoutExercises.Select(we => new WorkoutExercise
        {
            WorkoutId = workoutId,
            ExerciseId = we.ExerciseId,
            Order = we.Order,
            Sets = we.Sets.Select(ws => new WorkoutSet
            {
                WarmUp = ws.WarmUp,
                UntilFailure = ws.UntilFailure,
                MinRepetitions = ws.MinRepetitions,
                MaxRepetitions = ws.MaxRepetitions,
                ResetTime = ws.ResetTime,
                Weight = ws.Weight,
                Order = ws.Order
            }).ToList()
        }).ToList();
}
