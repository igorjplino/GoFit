using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Queries;

public record GetAllExercisesQuery(
    int PageNumber,
    int PageSize)
    : IRequest<Result<List<ExerciseDto>>>
{ }

public class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, Result<List<ExerciseDto>>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetAllExercisesQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<List<ExerciseDto>>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Exercise> exercises = await _exerciseRepository.GetAllAsync();
        
        if (exercises is null)
            return Enumerable.Empty<ExerciseDto>().ToList();

        return exercises.Select(x => new ExerciseDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }).ToList();
    }
}