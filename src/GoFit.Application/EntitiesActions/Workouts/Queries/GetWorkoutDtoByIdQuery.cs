using GoFit.Application.EntitiesActions.Workouts.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Queries;
public record GetWorkoutDtoByIdQuery : IRequest<WorkoutDto?>
{
    public Guid Id { get; set; }
}

public class GetWorkoutPlanDtoByIdQueryHandler : IRequestHandler<GetWorkoutDtoByIdQuery, WorkoutDto?>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutPlanDtoByIdQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<WorkoutDto?> Handle(GetWorkoutDtoByIdQuery request, CancellationToken cancellationToken)
    {
        Workout? workout = await _workoutRepository.GetAsync(request.Id);

        if (workout is null)
            return null;

        return new WorkoutDto
        {
        };
    }
}
