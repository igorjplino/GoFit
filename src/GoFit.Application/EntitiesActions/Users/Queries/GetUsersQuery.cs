using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Users.Dtos;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Application.EntitiesActions.Users.Queries;

public record GetUsersQuery : IRequest<Result<List<UserSummaryDto>>>
{ }

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserSummaryDto>>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUsersQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<List<UserSummaryDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken);
        var result = new List<UserSummaryDto>(users.Count);

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new UserSummaryDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? string.Empty
            });
        }

        return result;
    }
}
