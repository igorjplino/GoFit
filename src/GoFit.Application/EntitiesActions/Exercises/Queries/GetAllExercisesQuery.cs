using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using LanguageExt;
using LanguageExt.Common;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Queries;

public record GetAllExercisesQuery : IRequest<Result<Seq<ExerciseDto>>>
{ }

public class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, Result<Seq<ExerciseDto>>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetAllExercisesQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<Seq<ExerciseDto>>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Exercise> exercises = await _exerciseRepository.GetAllAsync();

        if (exercises is null)
            return Seq.empty<ExerciseDto>();

        return exercises.Select(x => new ExerciseDto
        {
            Name = x.Name,
            Description = x.Description
        }).ToSeq();
    }
}