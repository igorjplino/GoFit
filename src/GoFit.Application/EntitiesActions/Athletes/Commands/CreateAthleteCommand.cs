using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Athletes.Commands;
public record CreateAthleteCommand
    (string Name)
    : IRequest<Result<Guid>>
{ }

public class CreateAthleteCommandHandler : IRequestHandler<CreateAthleteCommand, Result<Guid>>
{
    private readonly IAthleteRepository _athleteRepository;

    public CreateAthleteCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task<Result<Guid>> Handle(CreateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = new Athlete
        {
            Name = request.Name
        };

        return await _athleteRepository.CreateAsync(athlete);
    }
}
