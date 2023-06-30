using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Commands;

public record CreateExerciseCommand : IRequest<ValidatorResponse<Guid>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CreateExerciseCommandhandler : IRequestHandler<CreateExerciseCommand, ValidatorResponse<Guid>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public CreateExerciseCommandhandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<ValidatorResponse<Guid>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = new Exercise
        {
            Name = request.Name,
            Description = request.Description
        };

        Guid id = await _exerciseRepository.CreateAsync(exercise);

        return ValidatorResponse<Guid>.Success(id);
    }
}
