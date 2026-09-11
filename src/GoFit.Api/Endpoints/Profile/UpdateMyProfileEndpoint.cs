using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Profile.Commands;
using GoFit.Application.EntitiesActions.Profile.Dtos;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Profile;

public class UpdateMyProfileEndpoint :
    BaseEndpoint<UpdateMyProfileCommand, ProfileDto>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateMyProfileEndpoint(
        ILogger<UpdateMyProfileEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Put("Profile/Me");
        Permissions(AppPermissions.Profile.Edit);
    }

    public override async Task HandleAsync(UpdateMyProfileCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with
        {
            AppUserId = appUser?.Id ?? string.Empty
        };

        Result<ProfileDto> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
