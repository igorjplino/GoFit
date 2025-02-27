﻿using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Athletes.Commands;

namespace GoFit.Api.Endpoints.Athlete;

public class CreateAthleteEndpoint :
    BaseEndpoint<CreateAthleteCommand, Guid>
{
    public CreateAthleteEndpoint(ILogger<CreateAthleteEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Post("Athlete");
    }

    public override async Task HandleAsync(CreateAthleteCommand req, CancellationToken ct)
    {
        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
