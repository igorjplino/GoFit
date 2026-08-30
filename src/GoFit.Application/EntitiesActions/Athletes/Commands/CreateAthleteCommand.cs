using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Domain.Enums;
using MediatR;

namespace GoFit.Application.EntitiesActions.Athletes.Commands;
public record CreateAthleteCommand
    (string AppUserId, string Name, string? Email)
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
            Name = request.Name,
            DisplayName = request.Name,
            Email = request.Email,
            Type = PersonType.Athlete,
            AppUserId = request.AppUserId
        };

        return await _athleteRepository.CreateAsync(athlete);
    }
}
