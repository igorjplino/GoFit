using GoFit.Application.Interfaces;
using GoFit.Application.Workouts.Dtos;
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
        WorkoutDto? workout = null;//_workoutPlanRepository.GetTodoItem(request.Id);

        if (workout is null)
            return Task.FromResult(null as WorkoutDto);

        return Task.FromResult<WorkoutDto?>(new WorkoutDto { });
    }
}
