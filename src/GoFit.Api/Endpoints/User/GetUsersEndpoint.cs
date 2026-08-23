using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Users.Dtos;
using GoFit.Application.EntitiesActions.Users.Queries;
using GoFit.Domain.Authorization;

namespace GoFit.Api.Endpoints.User;

public class GetUsersEndpoint :
    BaseEndpointWithoutRequest<List<UserSummaryDto>>
{
    public GetUsersEndpoint(ILogger<GetUsersEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("User");
        Permissions(AppPermissions.Users.View);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Result<List<UserSummaryDto>> result = await Mediator.Send(new GetUsersQuery(), ct);

        await HandleResultResponse(result, ct);
    }
}
