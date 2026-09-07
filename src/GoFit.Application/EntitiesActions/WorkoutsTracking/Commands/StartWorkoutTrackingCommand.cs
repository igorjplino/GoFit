using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

public record StartWorkoutTrackingCommand(
    Guid WorkoutId,
    string? Note,
    IEnumerable<WorkoutSetTrackingDto> Sets,
    string AppUserId = "")
    : IRequest<Result<Guid>>
{ }

public class StartWorkoutCommandHandler : IRequestHandler<StartWorkoutTrackingCommand, Result<Guid>>
{
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;
    private readonly IAthleteRepository _athleteRepository;

    public StartWorkoutCommandHandler(IWorkoutTrackingRepository workoutTrackingRepository, IAthleteRepository athleteRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<Guid>> Handle(StartWorkoutTrackingCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        var workoutTracking = ToEntity(request, athlete!.Id);

        return await _workoutTrackingRepository.CreateAsync(workoutTracking);
    }

    private static WorkoutTracking ToEntity(StartWorkoutTrackingCommand request, Guid athleteId)
        => new()
        {
            WorkoutId = request.WorkoutId,
            AthleteId = athleteId,
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
