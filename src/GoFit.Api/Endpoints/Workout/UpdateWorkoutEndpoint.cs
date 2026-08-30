using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Commands;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Workout;

public class UpdateWorkoutEndpoint :
    BaseEndpoint<UpdateWorkoutCommand, Guid?>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateWorkoutEndpoint(
        ILogger<UpdateWorkoutEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Put("Workout/{id}");
        Permissions(AppPermissions.Training.EditWorkouts);
    }

    public override async Task HandleAsync(UpdateWorkoutCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with
        {
            WorkoutId = Route<Guid>("id"),
            AppUserId = appUser?.Id ?? string.Empty
        };

        Result<Guid?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
