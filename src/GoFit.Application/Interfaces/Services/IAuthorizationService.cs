using GoFit.Domain.Entities.Identity;

namespace GoFit.Application.Interfaces.Services;

public interface IAuthorizationService
{
    string GenerateToken(AppUser user);
}