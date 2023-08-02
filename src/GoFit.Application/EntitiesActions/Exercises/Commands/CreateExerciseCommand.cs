using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using LanguageExt.Common;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Commands;

public record CreateExerciseCommand : IRequest<Result<Guid>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CreateExerciseCommandhandler : IRequestHandler<CreateExerciseCommand, Result<Guid>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public CreateExerciseCommandhandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<Guid>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = new Exercise
        {
            Name = request.Name,
            Description = request.Description
        };

        return await _exerciseRepository.CreateAsync(exercise);
    }
}
