using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record UpdateWorkoutSetCommand(
    Guid WorkoutsTrackingId,
    int Order,
    int Repetitions,
    float Weight,
    string AppUserId = "")
    : IRequest<Result<WorkoutTrackingDto>>, IWorkoutTrackingCommand
{ }

public class UpdateWorkoutSetCommandHandler : IRequestHandler<UpdateWorkoutSetCommand, Result<WorkoutTrackingDto>>
{
    private readonly IWorkoutSetTrackingRepository _workoutSetTrackingRepository;
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;

    public UpdateWorkoutSetCommandHandler(
        IWorkoutSetTrackingRepository workoutSetTrackingRepository,
        IWorkoutTrackingRepository workoutTrackingRepository)
    {
        _workoutSetTrackingRepository = workoutSetTrackingRepository;
        _workoutTrackingRepository = workoutTrackingRepository;
    }

    public async Task<Result<WorkoutTrackingDto>> Handle(UpdateWorkoutSetCommand request, CancellationToken cancellationToken)
    {
        // Validation already guaranteed the tracking is the caller's, in progress, and has a set at this position.
        var sets = await _workoutSetTrackingRepository.ListByTrackingIdAsync(request.WorkoutsTrackingId);

        var set = sets.First(o => o.Order == request.Order);

        await _workoutSetTrackingRepository.UpdateSetAsync(set.Id, request.Repetitions, request.Weight);

        var workoutTracking = await _workoutTrackingRepository.GetWithSetsAsync(request.WorkoutsTrackingId);

        return WorkoutTrackingDtoMapper.ToDto(workoutTracking!);
    }
}
