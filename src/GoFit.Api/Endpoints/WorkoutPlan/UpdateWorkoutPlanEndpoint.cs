using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class UpdateWorkoutPlanEndpoint :
    BaseEndpoint<UpdateWorkoutPlanCommand, Guid?>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateWorkoutPlanEndpoint(
        ILogger<UpdateWorkoutPlanEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Put("WorkoutPlan/{id}");
        Permissions(AppPermissions.Training.EditWorkoutPlans);
    }

    public override async Task HandleAsync(UpdateWorkoutPlanCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with
        {
            WorkoutPlanId = Route<Guid>("id"),
            AppUserId = appUser?.Id ?? string.Empty
        };

        Result<Guid?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
