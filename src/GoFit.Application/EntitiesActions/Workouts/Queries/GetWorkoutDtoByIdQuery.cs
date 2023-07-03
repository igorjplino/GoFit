using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Workouts.Queries;
public record GetWorkoutDtoByIdQuery : IRequest<ValidatorResponse<WorkoutDto?>>
{
    public Guid Id { get; set; }
}

public class GetWorkoutPlanDtoByIdQueryHandler : IRequestHandler<GetWorkoutDtoByIdQuery, ValidatorResponse<WorkoutDto?>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutPlanDtoByIdQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<ValidatorResponse<WorkoutDto?>> Handle(GetWorkoutDtoByIdQuery request, CancellationToken cancellationToken)
    {
        Workout? workout = await _workoutRepository.GetAsync(request.Id);

        if (workout is null)
            return ValidatorResponse<WorkoutDto?>.Success(null);

        var workoutDto = new WorkoutDto
        {
        };

        return ValidatorResponse<WorkoutDto?>.Success(workoutDto);
    }
}
