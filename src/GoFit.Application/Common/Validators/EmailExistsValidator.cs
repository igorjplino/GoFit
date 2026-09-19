using FluentValidation;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Application.Common.Validators;

/// <summary>
/// Passes only while no account uses the email yet - an existing email is the failure case.
/// </summary>
public class EmailExistsValidator : AbstractValidator<string>
{
    public EmailExistsValidator(UserManager<AppUser> userManager)
    {
        RuleFor(x => x)
            .MustAsync(async (email, ct) => await userManager.FindByEmailAsync(email) is null)
            .WithMessage("Email is already taken");
    }
}
