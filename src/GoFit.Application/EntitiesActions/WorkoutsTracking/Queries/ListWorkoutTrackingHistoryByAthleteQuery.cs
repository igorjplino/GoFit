using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Queries;

public record ListWorkoutTrackingHistoryByAthleteQuery(string AppUserId)
    : IRequest<Result<List<WorkoutTrackingSummaryDto>>>
{ }

public class ListWorkoutTrackingHistoryByAthleteQueryHandler
    : IRequestHandler<ListWorkoutTrackingHistoryByAthleteQuery, Result<List<WorkoutTrackingSummaryDto>>>
{
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;
    private readonly IAthleteRepository _athleteRepository;

    public ListWorkoutTrackingHistoryByAthleteQueryHandler(
        IWorkoutTrackingRepository workoutTrackingRepository,
        IAthleteRepository athleteRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<List<WorkoutTrackingSummaryDto>>> Handle(ListWorkoutTrackingHistoryByAthleteQuery request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetByAppUserIdAsync(request.AppUserId);

        if (athlete is null)
            return Enumerable.Empty<WorkoutTrackingSummaryDto>().ToList();

        List<WorkoutTracking> history = await _workoutTrackingRepository.ListHistoryByAthleteIdAsync(athlete.Id);

        if (history is null)
            return Enumerable.Empty<WorkoutTrackingSummaryDto>().ToList();

        return history.Select(WorkoutTrackingDtoMapper.ToSummaryDto).ToList();
    }
}
