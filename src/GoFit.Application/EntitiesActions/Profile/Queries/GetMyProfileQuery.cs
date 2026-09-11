using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Profile.Dtos;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Application.EntitiesActions.Profile.Queries;

public record GetMyProfileQuery(string AppUserId) : IRequest<Result<ProfileDto>>
{ }

public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, Result<ProfileDto>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetMyProfileQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<ProfileDto>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.AppUserId))
        {
            return default;
        }

        AppUser? user = await _userManager.FindByIdAsync(request.AppUserId);

        if (user is null)
        {
            return default;
        }

        // The name is read from the identity user because that is the copy the header greeting
        // and Account/User-Info already show, and an Athlete row may not exist (e.g. admins).
        return new ProfileDto
        {
            Name = user.DisplayName,
            Email = user.Email ?? string.Empty
        };
    }
}
