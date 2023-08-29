﻿using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Dtos;
using GoFit.Application.EntitiesActions.Workouts.Queries;

namespace GoFit.Api.Endpoints.Workout;

public class GetWorkoutByIdEndpoint :
    BaseEndpoint<GetWorkoutDtoByIdQuery, WorkoutDto?>
{
    public override void Configure()
    {
        Get("Workout/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetWorkoutDtoByIdQuery req, CancellationToken ct)
    {
        Result<WorkoutDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
