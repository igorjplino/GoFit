using GoFit.Application.Common;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace GoFit.Application.EntitiesActions.Accounts.Commands;
public record LoginCommand(
    string Email,
    string Password)
    : IRequest<Result<bool>>
{ }

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<bool>>
{
    private readonly UserManager<AppUser> _userManager;

    public LoginCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return false;
        }

        return true;
    }
}
