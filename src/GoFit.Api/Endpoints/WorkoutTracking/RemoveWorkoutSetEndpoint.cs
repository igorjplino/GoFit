using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class RemoveWorkoutSetEndpoint :
    BaseEndpoint<RemoveWorkoutSetCommand, WorkoutTrackingDto>
{
    private readonly UserManager<AppUser> _userManager;

    public RemoveWorkoutSetEndpoint(
        ILogger<RemoveWorkoutSetEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Delete("WorkoutTracking/{WorkoutsTrackingId}/sets/{Order}");
        Permissions(AppPermissions.Training.EditWorkoutTracking);
    }

    public override async Task HandleAsync(RemoveWorkoutSetCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with { AppUserId = appUser?.Id ?? string.Empty };

        Result<WorkoutTrackingDto> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
