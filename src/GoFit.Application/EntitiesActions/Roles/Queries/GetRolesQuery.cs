using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Roles.Dtos;
using GoFit.Domain.Authorization;
using MediatR;

namespace GoFit.Application.EntitiesActions.Roles.Queries;

public record GetRolesQuery : IRequest<Result<List<RoleDto>>>
{ }

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
{
    public Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = AppRoles.All
            .Select(role => new RoleDto
            {
                Name = role,
                Permissions = RolePermissions.For(role).ToArray()
            })
            .ToList();

        Result<List<RoleDto>> result = roles;

        return Task.FromResult(result);
    }
}
