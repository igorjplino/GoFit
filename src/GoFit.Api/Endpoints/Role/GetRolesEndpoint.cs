using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Roles.Dtos;
using GoFit.Application.EntitiesActions.Roles.Queries;
using GoFit.Domain.Authorization;

namespace GoFit.Api.Endpoints.Role;

public class GetRolesEndpoint :
    BaseEndpointWithoutRequest<List<RoleDto>>
{
    public GetRolesEndpoint(ILogger<GetRolesEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("Role");
        Permissions(AppPermissions.RoleManagement.View);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Result<List<RoleDto>> result = await Mediator.Send(new GetRolesQuery(), ct);

        await HandleResultResponse(result, ct);
    }
}
