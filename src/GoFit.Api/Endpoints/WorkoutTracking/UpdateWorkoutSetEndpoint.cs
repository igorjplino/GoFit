using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class UpdateWorkoutSetEndpoint :
    BaseEndpoint<UpdateWorkoutSetCommand, WorkoutTrackingDto>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateWorkoutSetEndpoint(
        ILogger<UpdateWorkoutSetEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Put("WorkoutTracking/{WorkoutsTrackingId}/sets/{Order}");
        Permissions(AppPermissions.Training.EditWorkoutTracking);
    }

    public override async Task HandleAsync(UpdateWorkoutSetCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with { AppUserId = appUser?.Id ?? string.Empty };

        Result<WorkoutTrackingDto> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
