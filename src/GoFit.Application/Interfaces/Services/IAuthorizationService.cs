using GoFit.Domain.Entities.Identity;

namespace GoFit.Application.Interfaces.Services;

public interface IAuthorizationService
{
    GeneratedToken GenerateToken(AppUser user, IList<string> roles);
}

public record GeneratedToken(string Value, DateTime ExpiresAt);
