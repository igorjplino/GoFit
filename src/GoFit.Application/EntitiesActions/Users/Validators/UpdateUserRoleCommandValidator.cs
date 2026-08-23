using FluentValidation;
using GoFit.Application.EntitiesActions.Users.Commands;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Application.EntitiesActions.Users.Validators;
public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserRoleCommandValidator(UserManager<AppUser> userManager)
    {
        _userManager = userManager;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.UserId)
            .MustAsync(UserExists).WithMessage("User not found");

        RuleFor(x => x.Role)
            .Must(role => AppRoles.All.Contains(role)).WithMessage("'{PropertyValue}' is not a known role");

        RuleFor(x => x)
            .MustAsync(NotRemovingLastAdmin)
            .WithMessage("Cannot change this user's role - they are the last remaining administrator")
            .WithName("Role");
    }

    private async Task<bool> UserExists(string userId, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user is not null;
    }

    private async Task<bool> NotRemovingLastAdmin(UpdateUserRoleCommand command, CancellationToken ct)
    {
        if (command.Role == AppRoles.Admin)
        {
            return true;
        }

        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user is null)
        {
            return true;
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (!currentRoles.Contains(AppRoles.Admin))
        {
            return true;
        }

        var admins = await _userManager.GetUsersInRoleAsync(AppRoles.Admin);

        return admins.Count > 1;
    }
}
