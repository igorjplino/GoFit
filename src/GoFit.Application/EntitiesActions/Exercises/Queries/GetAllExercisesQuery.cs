using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.Interfaces;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Queries;

public record GetAllExercisesQuery : IRequest<IEnumerable<ExerciseDto>>
{ }

public class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, IEnumerable<ExerciseDto>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetAllExercisesQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<IEnumerable<ExerciseDto>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
    {
        var exercises = await _exerciseRepository.GetAllAsync();

        if (exercises is null)
            return Enumerable.Empty<ExerciseDto>();

        return exercises.Select(x => new ExerciseDto
        {
            Name = x.Name,
            Description = x.Description
        });
    }
}