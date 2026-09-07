using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record CancelWorkoutTrackingCommand(
    Guid WorkoutsTrackingId,
    string AppUserId = "")
    : IRequest<Result<Guid>>
{ }

public class CancelWorkoutTrackingCommandHandler : IRequestHandler<CancelWorkoutTrackingCommand, Result<Guid>>
{
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;

    public CancelWorkoutTrackingCommandHandler(IWorkoutTrackingRepository workoutTrackingRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
    }

    public async Task<Result<Guid>> Handle(CancelWorkoutTrackingCommand request, CancellationToken cancellationToken)
    {
        await _workoutTrackingRepository.CancelWorkoutTrackingAsync(request.WorkoutsTrackingId, DateTime.UtcNow);

        return request.WorkoutsTrackingId;
    }
}
