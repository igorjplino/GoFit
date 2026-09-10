using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Queries;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class GetMyWorkoutHistoryEndpoint :
    BaseEndpointWithoutRequest<List<WorkoutTrackingSummaryDto>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetMyWorkoutHistoryEndpoint(
        ILogger<GetMyWorkoutHistoryEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get("WorkoutTracking/mine/history");
        Permissions(AppPermissions.Training.ViewWorkoutTracking);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        Result<List<WorkoutTrackingSummaryDto>> result = await Mediator.Send(new ListWorkoutTrackingHistoryByAthleteQuery(appUser?.Id ?? string.Empty), ct);

        await HandleResultResponse(result, ct);
    }
}
