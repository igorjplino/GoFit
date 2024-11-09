using FastEndpoints;
using FluentValidation;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Account.Validators;

public class RegisterValidator : Validator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .SetValidator(new EmailExistsValidator());

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
