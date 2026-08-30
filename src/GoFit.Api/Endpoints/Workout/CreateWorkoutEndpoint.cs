using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Commands;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Workout;

public class CreateWorkoutEndpoint :
    BaseEndpoint<CreateWorkoutCommand, Guid>
{
    private readonly UserManager<AppUser> _userManager;

    public CreateWorkoutEndpoint(
        ILogger<CreateWorkoutEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("Workout");
        Permissions(AppPermissions.Training.CreateWorkouts);
    }

    public override async Task HandleAsync(CreateWorkoutCommand req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with { AppUserId = appUser?.Id ?? string.Empty };

        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
