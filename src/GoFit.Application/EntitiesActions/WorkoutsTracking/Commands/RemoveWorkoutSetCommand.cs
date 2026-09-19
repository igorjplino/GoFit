using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Application.Models;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record RemoveWorkoutSetCommand(
    Guid WorkoutsTrackingId,
    int Order,
    string AppUserId = "")
    : IRequest<Result<WorkoutTrackingDto>>, IWorkoutTrackingCommand
{ }

public class RemoveWorkoutSetCommandHandler : IRequestHandler<RemoveWorkoutSetCommand, Result<WorkoutTrackingDto>>
{
    private readonly IWorkoutSetTrackingRepository _workoutSetTrackingRepository;
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;

    public RemoveWorkoutSetCommandHandler(
        IWorkoutSetTrackingRepository workoutSetTrackingRepository,
        IWorkoutTrackingRepository workoutTrackingRepository)
    {
        _workoutSetTrackingRepository = workoutSetTrackingRepository;
        _workoutTrackingRepository = workoutTrackingRepository;
    }

    public async Task<Result<WorkoutTrackingDto>> Handle(RemoveWorkoutSetCommand request, CancellationToken cancellationToken)
    {
        // Validation already guaranteed the tracking is the caller's, in progress, and has a set at this position.
        var sets = await _workoutSetTrackingRepository.ListByTrackingIdAsync(request.WorkoutsTrackingId);

        var removedSet = sets.First(o => o.Order == request.Order);

        await _workoutSetTrackingRepository.RemoveAndResequenceAsync(removedSet.Id, Resequence(sets, removedSet.Id));

        var workoutTracking = await _workoutTrackingRepository.GetWithSetsAsync(request.WorkoutsTrackingId);

        return WorkoutTrackingDtoMapper.ToDto(workoutTracking!);
    }

    // Removing a set from the middle would leave a gap; closing it keeps every later set paired with the planned set
    // at its new position, and the next logged set from colliding with an existing order. Sets that don't move are
    // left out, so removing the last set writes nothing.
    private static List<WorkoutSetPosition> Resequence(List<WorkoutSetTracking> sets, Guid removedSetId)
    {
        var resequenced = new List<WorkoutSetPosition>();
        var order = 0;

        foreach (var set in sets.Where(o => o.Id != removedSetId))
        {
            if (set.Order != order)
            {
                resequenced.Add(new WorkoutSetPosition(set.Id, order));
            }

            order++;
        }

        return resequenced;
    }
}
