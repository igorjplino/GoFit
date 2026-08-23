using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Users.Commands;
using GoFit.Domain.Authorization;

namespace GoFit.Api.Endpoints.User;

public class UpdateUserRoleEndpoint :
    BaseEndpoint<UpdateUserRoleCommand, string>
{
    public UpdateUserRoleEndpoint(ILogger<UpdateUserRoleEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Put("User/{UserId}/Role");
        Permissions(AppPermissions.RoleManagement.ManageUserRoles);
    }

    public override async Task HandleAsync(UpdateUserRoleCommand req, CancellationToken ct)
    {
        Result<string> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
