using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Queries;

public record GetWorkoutDtoByIdQuery : IRequest<Result<WorkoutDto?>>
{
    public Guid Id { get; set; }
}

public class GetWorkoutPlanDtoByIdQueryHandler : IRequestHandler<GetWorkoutDtoByIdQuery, Result<WorkoutDto?>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutPlanDtoByIdQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<Result<WorkoutDto?>> Handle(GetWorkoutDtoByIdQuery request, CancellationToken cancellationToken)
    {
        Workout? workout = await _workoutRepository.GetAsync(request.Id);

        if (workout is null)
            return default;

        var workoutDto = new WorkoutDto
        {
        };

        return workoutDto;
    }
}
