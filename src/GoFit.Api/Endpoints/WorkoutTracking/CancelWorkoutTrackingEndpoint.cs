using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class CancelWorkoutTrackingEndpoint :
    BaseEndpoint<CancelWorkoutTrackingCommand, Guid>
{
    private readonly UserManager<AppUser> _userManager;

    public CancelWorkoutTrackingEndpoint(
        ILogger<CancelWorkoutTrackingEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Put("WorkoutTracking/{WorkoutsTrackingId}/cancel");
        Permissions(AppPermissions.Training.EditWorkoutTracking);
    }

    public override async Task HandleAsync(CancelWorkoutTrackingCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with { AppUserId = appUser?.Id ?? string.Empty };

        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
