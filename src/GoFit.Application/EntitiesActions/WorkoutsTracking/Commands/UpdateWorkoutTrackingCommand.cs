using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record UpdateWorkoutTrackingCommand(
    Guid WorkoutsTrackingId,
    DateTime StartWorkoutDate,
    DateTime? EndWorkoutDate,
    string? Note,
    IEnumerable<WorkoutSetTrackingDto> Sets)
    : IRequest<Result<UpdateWorkoutTrackingCommand>>
{ }

public class UpdateWorkoutCommandHandler : IRequestHandler<UpdateWorkoutTrackingCommand, Result<UpdateWorkoutTrackingCommand>>
{
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;

    public UpdateWorkoutCommandHandler(IWorkoutTrackingRepository workoutTrackingRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
    }

    public async Task<Result<UpdateWorkoutTrackingCommand>> Handle(UpdateWorkoutTrackingCommand request, CancellationToken cancellationToken)
    {
        var workoutTracking = ToEntity(request);

        await _workoutTrackingRepository.UpdateWorkoutTrackingAsync(workoutTracking);

        return request;
    }

    private WorkoutTracking ToEntity(UpdateWorkoutTrackingCommand request)
        => new()
        {
            Id = request.WorkoutsTrackingId,
            StartWorkoutDate = request.StartWorkoutDate,
            EndWorkoutDate = request.EndWorkoutDate,
            Note = request.Note,
            Sets = request.Sets.Select(o => new WorkoutSetTracking
            {
                WorkoutTrackingId = request.WorkoutsTrackingId,
                Repetitions = o.Repetitions,
                Weight = o.Weight,
                Order = o.Order
            }).ToList()
        };
}
