using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Dtos;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Queries;

public record GetWorkoutTrackingByIdQuery : IRequest<Result<WorkoutTrackingDto?>>
{
    public Guid Id { get; set; }
}

public class GetWorkoutTrackingByIdQueryHandler : IRequestHandler<GetWorkoutTrackingByIdQuery, Result<WorkoutTrackingDto?>>
{
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;

    public GetWorkoutTrackingByIdQueryHandler(IWorkoutTrackingRepository workoutTrackingRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
    }

    public async Task<Result<WorkoutTrackingDto?>> Handle(GetWorkoutTrackingByIdQuery request, CancellationToken cancellationToken)
    {
        WorkoutTracking? workoutTracking = await _workoutTrackingRepository.GetWithSetsAsync(request.Id);

        if (workoutTracking is null)
            return default;

        return ToDto(workoutTracking);
    }

    private static WorkoutTrackingDto ToDto(WorkoutTracking workoutTracking)
        => new()
        {
            WorkoutId = workoutTracking.Id,
            StartWorkoutDate = workoutTracking.StartWorkoutDate,
            EndWorkoutDate = workoutTracking.EndWorkoutDate,
            Note = workoutTracking.Note,
            Sets = workoutTracking.Sets.Select(o => new WorkoutSetTrackingDto
            {
                Order = o.Order,
                Repetitions = o.Repetitions,
                Weight = o.Weight
            }).ToList()
        };
}
