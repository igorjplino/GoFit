using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Application.Models;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Queries;

public record GetAllExercisesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Name = null)
    : BaseFilters(PageNumber, PageSize),
        IRequest<Result<PaginatedResult<ExerciseDto>>>
{ }

public class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, Result<PaginatedResult<ExerciseDto>>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetAllExercisesQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<PaginatedResult<ExerciseDto>>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
    {
        (IEnumerable<Exercise> exercises, int total) = await _exerciseRepository.ListExercisesAsync(request);

        return new PaginatedResult<ExerciseDto>(
            request.PageNumber,
            request.PageSize,
            total,
            exercises.Select(x => new ExerciseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList().AsReadOnly());
    }
}