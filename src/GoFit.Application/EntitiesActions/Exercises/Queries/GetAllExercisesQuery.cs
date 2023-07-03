using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.Interfaces;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Queries;

public record GetAllExercisesQuery : IRequest<ValidatorResponse<IEnumerable<ExerciseDto>>>
{ }

public class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, ValidatorResponse<IEnumerable<ExerciseDto>>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetAllExercisesQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<ValidatorResponse<IEnumerable<ExerciseDto>>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
    {
        var exercises = await _exerciseRepository.GetAllAsync();

        if (exercises is null)
            return ValidatorResponse<IEnumerable<ExerciseDto>>.Success(Enumerable.Empty<ExerciseDto>());

        IEnumerable<ExerciseDto> exercisesDto = exercises.Select(x => new ExerciseDto
        {
            Name = x.Name,
            Description = x.Description
        });

        return ValidatorResponse<IEnumerable<ExerciseDto>>.Success(exercisesDto);
    }
}