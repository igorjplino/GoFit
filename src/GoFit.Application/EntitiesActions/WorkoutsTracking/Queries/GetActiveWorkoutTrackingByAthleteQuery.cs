using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Queries;

public record GetActiveWorkoutTrackingByAthleteQuery(string AppUserId)
    : IRequest<Result<WorkoutTrackingDto?>>
{ }

public class GetActiveWorkoutTrackingByAthleteQueryHandler : IRequestHandler<GetActiveWorkoutTrackingByAthleteQuery, Result<WorkoutTrackingDto?>>
{
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;
    private readonly IAthleteRepository _athleteRepository;

    public GetActiveWorkoutTrackingByAthleteQueryHandler(IWorkoutTrackingRepository workoutTrackingRepository, IAthleteRepository athleteRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<WorkoutTrackingDto?>> Handle(GetActiveWorkoutTrackingByAthleteQuery request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        if (athlete is null)
            return default;

        WorkoutTracking? workoutTracking = await _workoutTrackingRepository.GetActiveByAthleteIdAsync(athlete.Id);

        if (workoutTracking is null)
            return default;

        return WorkoutTrackingDtoMapper.ToDto(workoutTracking);
    }
}
