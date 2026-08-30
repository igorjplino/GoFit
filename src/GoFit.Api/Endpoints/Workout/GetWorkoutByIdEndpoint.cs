using GoFit.Api.Extensions;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Dtos;
using GoFit.Application.EntitiesActions.Workouts.Queries;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Workout;

public class GetWorkoutByIdEndpoint :
    BaseEndpoint<GetWorkoutDtoByIdQuery, WorkoutDto?>
{
    private readonly UserManager<AppUser> _userManager;

    public GetWorkoutByIdEndpoint(
        ILogger<GetWorkoutByIdEndpoint> logger,
        UserManager<AppUser> userManager)
        : base(logger)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Get("Workout/{id}");
        Permissions(AppPermissions.Training.ViewWorkouts);
    }

    public override async Task HandleAsync(GetWorkoutDtoByIdQuery req, CancellationToken ct)
    {
        var appUser = await _userManager.GetUser(User);

        req = req with { AppUserId = appUser?.Id ?? string.Empty };

        Result<WorkoutDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
