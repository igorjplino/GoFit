using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Queries;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class GetMyWorkoutPlansEndpoint :
    BaseEndpointWithoutRequest<List<WorkoutPlanDto>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetMyWorkoutPlansEndpoint(
        ILogger<GetMyWorkoutPlansEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get("WorkoutPlan/Mine");
        Permissions(AppPermissions.Training.ViewWorkoutPlans);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        Result<List<WorkoutPlanDto>> result = await Mediator.Send(new ListWorkoutPlansByIAthletedQuery(appUser?.Id ?? string.Empty), ct);

        await HandleResultResponse(result, ct);
    }
}
