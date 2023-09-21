using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Queries;

public record GetExerciseDtoByIdQuery : IRequest<Result<ExerciseDto?>>
{
    public Guid Id { get; set; }
}

public class GetExerciseDtoByIdQueryHandler : IRequestHandler<GetExerciseDtoByIdQuery, Result<ExerciseDto?>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExerciseDtoByIdQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<ExerciseDto?>> Handle(GetExerciseDtoByIdQuery request, CancellationToken cancellationToken)
    {
        Exercise? exercise = await _exerciseRepository.GetAsync(request.Id);

        if (exercise is null)
            return default;

        var exerciseDto = new ExerciseDto
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description
        };

        return exerciseDto;
    }
}
