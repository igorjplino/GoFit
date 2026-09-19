using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record LogWorkoutSetCommand(
    Guid WorkoutsTrackingId,
    int Repetitions,
    float Weight,
    string AppUserId = "")
    : IRequest<Result<WorkoutTrackingDto>>, IWorkoutTrackingCommand
{ }

public class LogWorkoutSetCommandHandler : IRequestHandler<LogWorkoutSetCommand, Result<WorkoutTrackingDto>>
{
    private readonly IWorkoutSetTrackingRepository _workoutSetTrackingRepository;
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;

    public LogWorkoutSetCommandHandler(
        IWorkoutSetTrackingRepository workoutSetTrackingRepository,
        IWorkoutTrackingRepository workoutTrackingRepository)
    {
        _workoutSetTrackingRepository = workoutSetTrackingRepository;
        _workoutTrackingRepository = workoutTrackingRepository;
    }

    public async Task<Result<WorkoutTrackingDto>> Handle(LogWorkoutSetCommand request, CancellationToken cancellationToken)
    {
        // Validation already guaranteed the tracking exists, belongs to the caller and is in progress.
        var sets = await _workoutSetTrackingRepository.ListByTrackingIdAsync(request.WorkoutsTrackingId);

        // Sets are completed in sequence, so a new set always takes the next position: the n-th logged set
        // pairs with the n-th planned set.
        await _workoutSetTrackingRepository.CreateAsync(new WorkoutSetTracking
        {
            WorkoutTrackingId = request.WorkoutsTrackingId,
            Order = sets.Count,
            Repetitions = request.Repetitions,
            Weight = request.Weight
        });

        var workoutTracking = await _workoutTrackingRepository.GetWithSetsAsync(request.WorkoutsTrackingId);

        return WorkoutTrackingDtoMapper.ToDto(workoutTracking!);
    }
}
