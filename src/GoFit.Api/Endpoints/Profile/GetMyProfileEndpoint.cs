using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Profile.Dtos;
using GoFit.Application.EntitiesActions.Profile.Queries;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Profile;

public class GetMyProfileEndpoint :
    BaseEndpointWithoutRequest<ProfileDto>
{
    private readonly UserManager<AppUser> _userManager;

    public GetMyProfileEndpoint(
        ILogger<GetMyProfileEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get("Profile/Me");
        Permissions(AppPermissions.Profile.View);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        Result<ProfileDto> result = await Mediator.Send(new GetMyProfileQuery(appUser?.Id ?? string.Empty), ct);

        await HandleResultResponse(result, ct);
    }
}
