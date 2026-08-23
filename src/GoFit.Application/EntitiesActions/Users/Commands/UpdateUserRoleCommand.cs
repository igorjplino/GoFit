using GoFit.Application.Common;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Application.EntitiesActions.Users.Commands;

public record UpdateUserRoleCommand(string UserId, string Role) : IRequest<Result<string>>
{ }

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Result<string>>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserRoleCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        var currentRoles = await _userManager.GetRolesAsync(user!);
        await _userManager.RemoveFromRolesAsync(user!, currentRoles);
        await _userManager.AddToRoleAsync(user!, request.Role);

        return request.UserId;
    }
}
