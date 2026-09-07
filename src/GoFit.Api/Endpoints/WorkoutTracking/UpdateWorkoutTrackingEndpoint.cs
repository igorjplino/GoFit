using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class UpdateWorkoutTrackingEndpoint :
    BaseEndpoint<UpdateWorkoutTrackingCommand, UpdateWorkoutTrackingCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateWorkoutTrackingEndpoint(
        ILogger<UpdateWorkoutTrackingEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Put("WorkoutTracking/{WorkoutsTrackingId}");
        Permissions(AppPermissions.Training.EditWorkoutTracking);
    }

    public override async Task HandleAsync(UpdateWorkoutTrackingCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with { AppUserId = appUser?.Id ?? string.Empty };

        Result<UpdateWorkoutTrackingCommand> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
