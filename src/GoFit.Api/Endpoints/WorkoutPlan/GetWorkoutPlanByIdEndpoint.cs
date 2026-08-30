using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Queries;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class GetWorkoutPlanByIdEndpoint :
    BaseEndpoint<GetWorkoutPlanDtoByIdQuery, WorkoutPlanDto?>
{
    private readonly UserManager<AppUser> _userManager;

    public GetWorkoutPlanByIdEndpoint(
        ILogger<GetWorkoutPlanByIdEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get("WorkoutPlan/{id}");
        Permissions(AppPermissions.Training.ViewWorkoutPlans);
    }

    public override async Task HandleAsync(GetWorkoutPlanDtoByIdQuery req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with { AppUserId = appUser?.Id ?? string.Empty };

        Result<WorkoutPlanDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
