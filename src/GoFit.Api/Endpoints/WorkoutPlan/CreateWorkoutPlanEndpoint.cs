using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class CreateWorkoutPlanEndpoint :
    BaseEndpoint<CreateWorkoutPlanCommand, Guid>
{
    private readonly UserManager<AppUser> _userManager;

    public CreateWorkoutPlanEndpoint(
        ILogger<CreateWorkoutPlanEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("WorkoutPlan");
        Permissions(AppPermissions.Training.CreateWorkoutPlans);
    }

    public override async Task HandleAsync(CreateWorkoutPlanCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with { AppUserId = appUser?.Id ?? string.Empty };

        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
