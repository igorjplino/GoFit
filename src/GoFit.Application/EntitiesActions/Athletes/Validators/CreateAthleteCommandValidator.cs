using FluentValidation;
using GoFit.Application.EntitiesActions.Athletes.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.Athletes.Validators;

public class CreateAthleteCommandValidator : AbstractValidator<CreateAthleteCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public CreateAthleteCommandValidator(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;

        RuleFor(x => x.AppUserId)
            .NotEmpty()
            .MustAsync(BeUnlinked).WithMessage("An athlete is already linked to this account.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200); // matches AthleteConfiguration's HasMaxLength(200) on Name
    }

    private async Task<bool> BeUnlinked(string appUserId, CancellationToken ct)
        => await _athleteRepository.GetByAppUserIdAsync(appUserId) is null;
}
