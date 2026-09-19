using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record FinishWorkoutTrackingCommand(
    Guid WorkoutsTrackingId,
    string AppUserId = "")
    : IRequest<Result<Guid>>, IWorkoutTrackingCommand
{ }

public class FinishWorkoutTrackingCommandHandler : IRequestHandler<FinishWorkoutTrackingCommand, Result<Guid>>
{
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;

    public FinishWorkoutTrackingCommandHandler(IWorkoutTrackingRepository workoutTrackingRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
    }

    public async Task<Result<Guid>> Handle(FinishWorkoutTrackingCommand request, CancellationToken cancellationToken)
    {
        await _workoutTrackingRepository.FinishWorkoutTrackingAsync(request.WorkoutsTrackingId, DateTime.UtcNow);

        return request.WorkoutsTrackingId;
    }
}
