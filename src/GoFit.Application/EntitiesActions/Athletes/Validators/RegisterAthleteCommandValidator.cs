using FluentValidation;
using GoFit.Application.Common.Validators;
using GoFit.Application.EntitiesActions.Athletes.Commands;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Application.EntitiesActions.Athletes.Validators;

public class RegisterAthleteCommandValidator : AbstractValidator<RegisterAthleteCommand>
{
    public RegisterAthleteCommandValidator(UserManager<AppUser> userManager)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200); // matches AthleteConfiguration's HasMaxLength(200) on Name

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .SetValidator(new EmailExistsValidator(userManager));

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
