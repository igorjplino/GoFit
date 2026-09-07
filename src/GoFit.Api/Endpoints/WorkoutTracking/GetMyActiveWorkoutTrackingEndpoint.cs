using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Queries;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class GetMyActiveWorkoutTrackingEndpoint :
    BaseEndpointWithoutRequest<WorkoutTrackingDto?>
{
    private readonly UserManager<AppUser> _userManager;

    public GetMyActiveWorkoutTrackingEndpoint(
        ILogger<GetMyActiveWorkoutTrackingEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    protected override bool NullResultIsNotFound => false;

    public override void Configure()
    {
        Get("WorkoutTracking/mine");
        Permissions(AppPermissions.Training.ViewWorkoutTracking);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        Result<WorkoutTrackingDto?> result = await Mediator.Send(new GetActiveWorkoutTrackingByAthleteQuery(appUser?.Id ?? string.Empty), ct);

        await HandleResultResponse(result, ct);
    }
}
