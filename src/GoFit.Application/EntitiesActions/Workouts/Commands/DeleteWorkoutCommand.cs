using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Commands;

public record DeleteWorkoutCommand(Guid WorkoutId, string AppUserId) : IRequest<Result<Guid?>>
{ }

public class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand, Result<Guid?>>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IAthleteRepository _athleteRepository;

    public DeleteWorkoutCommandHandler(IWorkoutRepository workoutRepository, IAthleteRepository athleteRepository)
    {
        _workoutRepository = workoutRepository;
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<Guid?>> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        if (athlete is null)
            return default;

        var workout = await _workoutRepository.GetWithDetailsAsync(request.WorkoutId);

        if (workout is null)
            return default;

        if (workout.WorkoutPlan.AthleteId != athlete.Id)
            return default;

        await _workoutRepository.DeleteAsync(workout.Id);

        return workout.Id;
    }
}
