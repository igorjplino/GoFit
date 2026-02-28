using System.Security.Claims;
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
        _key = configuration["Token:Key"]!;
    }

    public string GenerateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            // new Claim(ClaimTypes.Name, username), // Represents the user's name
            new(ClaimTypes.Email, user.Email!),  // The user's email
            // new Claim(JwtRegisteredClaimNames.Sub, userId), // The subject identifier
            // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
        };
        
        // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

        // var token = new JwtSecurityToken(
        //     claims: claims,
        //     expires: DateTime.UtcNow.AddMinutes(15),
        //     signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        //
        // return new JwtSecurityTokenHandler().WriteToken(token);
        
        return JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = _key;
                o.ExpireAt = DateTime.UtcNow.AddHours(1000);
                // o.User.Roles.Add("Manager", "Auditor");
                o.User.Claims.AddRange(claims);
                // o.User["UserId"] = "001"; //indexer based claim setting
            });
    }
}