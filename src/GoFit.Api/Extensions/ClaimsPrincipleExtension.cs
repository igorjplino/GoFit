using System.Security.Claims;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Api.Extensions;

public static class ClaimsPrincipleExtension
{
    public static async Task<AppUser?> GetUser(this UserManager<AppUser> userManager,
        ClaimsPrincipal user)
    {
        return await userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == user.GetEmail());
    }
    
    public static string? GetEmail(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email);
    }
}