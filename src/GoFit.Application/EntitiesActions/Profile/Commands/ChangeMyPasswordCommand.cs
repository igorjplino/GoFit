using GoFit.Application.Common;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Application.EntitiesActions.Profile.Commands;

public record ChangeMyPasswordCommand(string CurrentPassword, string NewPassword, string AppUserId = "") : IRequest<Result<bool>>
{ }

public class ChangeMyPasswordCommandHandler : IRequestHandler<ChangeMyPasswordCommand, Result<bool>>
{
    private readonly UserManager<AppUser> _userManager;

    public ChangeMyPasswordCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(ChangeMyPasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await _userManager.FindByIdAsync(request.AppUserId);

        if (user is null)
        {
            return new InvalidOperationException("User not found.");
        }

        IdentityResult identityResult = await _userManager.ChangePasswordAsync(
            user, request.CurrentPassword, request.NewPassword);

        if (!identityResult.Succeeded)
        {
            return new InvalidOperationException(
                string.Join(" ", identityResult.Errors.Select(error => error.Description)));
        }

        return true;
    }
}
