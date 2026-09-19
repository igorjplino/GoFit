using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Athletes.Commands;
using GoFit.Application.EntitiesActions.Athletes.Dtos;

namespace GoFit.Api.Endpoints.Account;

public class RegisterEndpoint :
    BaseEndpoint<RegisterAthleteCommand, RegisteredAthleteDto>
{
    public RegisterEndpoint(ILogger<RegisterEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Post("Account/Register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterAthleteCommand req, CancellationToken ct)
    {
        Result<RegisteredAthleteDto> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
