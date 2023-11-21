using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record StartWorkoutTrackingCommand : IRequest<Result<Guid>>
{
    public Guid WorkoutId { get; set; }
    public DateTime StartWorkoutDate { get; set; }
    public DateTime EndWorkoutDate { get; set; }
    public string? Note { get; set; }
    public ICollection<WorkoutSetTrackingDto> Sets { get; set; }
}

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
            StartWorkoutDate = request.StartWorkoutDate,
            EndWorkoutDate = request.EndWorkoutDate,
            Note = request.Note,
            Sets = request.Sets.Select(o => new WorkoutSetTracking
            {
                Repetitions = o.Repetitions,
                Weight = o.Weight,
                Order = o.Order
            }).ToList()
        };
}
