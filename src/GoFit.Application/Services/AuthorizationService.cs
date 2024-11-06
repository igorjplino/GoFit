using FastEndpoints.Security;
using GoFit.Application.Interfaces.Services;
using GoFit.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;

namespace GoFit.Application.Services;

public class AuthorizationService : IAuthorizationService 
{
    private readonly string _key;
    
    public AuthorizationService(IConfiguration configuration)
    {
        _key = configuration["Token:Key"];
    }

    public string GenerateToken(AppUser user)
    {
        return JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = _key;
                o.ExpireAt = DateTime.UtcNow.AddMinutes(1);
                // o.User.Roles.Add("Manager", "Auditor");
                // o.User.Claims.Add(("UserName", req.Username));
                // o.User["UserId"] = "001"; //indexer based claim setting
            });
    }
}