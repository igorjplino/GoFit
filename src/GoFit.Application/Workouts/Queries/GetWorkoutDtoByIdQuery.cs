using GoFit.Application.Interfaces;
using GoFit.Application.Workouts.Dtos;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.Workouts.Queries;
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

    public Task<WorkoutDto?> Handle(GetWorkoutDtoByIdQuery request, CancellationToken cancellationToken)
    {
        Workout? workout = _workoutRepository.Get(request.Id);

        if (workout is null)
            return Task.FromResult(null as WorkoutDto);

        var dto = new WorkoutDto
        {
        };

        return Task.FromResult<WorkoutDto?>(dto);
    }
}
