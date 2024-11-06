using FastEndpoints;
using FluentValidation;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Account.Validators;

public class EmailExistsValidator : Validator<string>
{
    public EmailExistsValidator()
    {
        RuleFor(x => x)
            .MustAsync(async (email, ct) =>
            {
                var userManager = Resolve<UserManager<AppUser>>();
            
                var appUser = await userManager.FindByEmailAsync(email);

                return appUser is null;
            }).WithMessage("Email is already taken");
    }
}