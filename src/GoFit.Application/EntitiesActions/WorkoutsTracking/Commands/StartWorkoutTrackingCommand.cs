using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record StartWorkoutTrackingCommand(
    Guid WorkoutId,
    string? Note,
    IEnumerable<WorkoutSetTrackingDto> Sets)
    : IRequest<Result<Guid>>
{ }

public class StartWorkoutCommandHandler : IRequestHandler<StartWorkoutTrackingCommand, Result<Guid>>
{
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;

    public StartWorkoutCommandHandler(IWorkoutTrackingRepository workoutTrackingRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
    }

    public async Task<Result<Guid>> Handle(StartWorkoutTrackingCommand request, CancellationToken cancellationToken)
    {
        var workoutTracking = ToEntity(request);

        return await _workoutTrackingRepository.CreateAsync(workoutTracking);
    }

    private WorkoutTracking ToEntity(StartWorkoutTrackingCommand request)
        => new()
        {
            WorkoutId = request.WorkoutId,
            StartWorkoutDate = DateTime.UtcNow,
            Note = request.Note,
            Sets = request.Sets.Select(o => new WorkoutSetTracking
            {
                Repetitions = o.Repetitions,
                Weight = o.Weight,
                Order = o.Order
            }).ToList()
        };
}
