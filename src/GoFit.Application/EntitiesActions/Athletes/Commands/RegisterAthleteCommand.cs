using FluentValidation;
using FluentValidation.Results;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Athletes.Dtos;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GoFit.Application.EntitiesActions.Athletes.Commands;

public record RegisterAthleteCommand(
    string Name,
    string Email,
    string Password)
    : IRequest<Result<RegisteredAthleteDto>>
{ }

public class RegisterAthleteCommandHandler : IRequestHandler<RegisterAthleteCommand, Result<RegisteredAthleteDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ISender _sender;
    private readonly ILogger<RegisterAthleteCommandHandler> _logger;

    public RegisterAthleteCommandHandler(
        UserManager<AppUser> userManager,
        ISender sender,
        ILogger<RegisterAthleteCommandHandler> logger)
    {
        _userManager = userManager;
        _sender = sender;
        _logger = logger;
    }

    public async Task<Result<RegisteredAthleteDto>> Handle(RegisterAthleteCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            DisplayName = request.Name,
            UserName = request.Email,
            Email = request.Email
        };

        IdentityResult created = await _userManager.CreateAsync(user, request.Password);

        if (!created.Succeeded)
        {
            // Identity's own rules (password strength, user name format) are reported like any other validation error.
            return new ValidationException(created.Errors.Select(error => new ValidationFailure(error.Code, error.Description)));
        }

        try
        {
            IdentityResult roleAssigned = await _userManager.AddToRoleAsync(user, AppRoles.Athlete);

            if (!roleAssigned.Succeeded)
            {
                return await UndoAccountAsync(user, new InvalidOperationException(
                    string.Join(" ", roleAssigned.Errors.Select(error => error.Description))));
            }

            // Goes through CreateAthleteCommand so the athlete gets the same validation as every other creation path.
            Result<Guid> athleteCreated = await _sender.Send(new CreateAthleteCommand(user.Id, request.Name, request.Email), cancellationToken);

            Exception? athleteError = athleteCreated.Match<Exception?>(succ => null, fail => fail);

            if (athleteError is not null)
            {
                return await UndoAccountAsync(user, athleteError);
            }
        }
        catch (Exception ex)
        {
            return await UndoAccountAsync(user, ex);
        }

        return new RegisteredAthleteDto
        {
            DisplayName = user.DisplayName,
            Email = request.Email
        };
    }

    // The account lives in the identity database and the athlete in the domain database, so they can't share a
    // transaction. Instead of leaving an account that can log in but owns no athlete, the account is removed again.
    private async Task<Result<RegisteredAthleteDto>> UndoAccountAsync(AppUser user, Exception error)
    {
        _logger.LogError(error, "Registering {Email} failed after the account was created - removing the account", user.Email);

        await _userManager.DeleteAsync(user);

        return error;
    }
}
