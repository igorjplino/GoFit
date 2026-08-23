using System.Security.Claims;
using GoFit.Domain.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace GoFit.Api.Authorization;

/// <summary>
/// Resolves the authenticated user's role claim(s) into permission claims on every request,
/// using the current <see cref="RolePermissions"/> mapping. This keeps permissions dynamic:
/// changing what a role grants takes effect immediately, without requiring users to re-login.
/// </summary>
public class PermissionClaimsTransformation : IClaimsTransformation
{
    private const string PermissionsResolvedClaimType = "permissions_resolved";
    public const string PermissionClaimType = "permissions";

    // FastEndpoints.Security writes role claims using the short "role" claim type (not the
    // ClaimTypes.Role URI), so both are checked here to reliably pick up the role claim(s).
    private static readonly string[] RoleClaimTypes = { "role", ClaimTypes.Role };

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
        {
            return Task.FromResult(principal);
        }

        if (identity.HasClaim(c => c.Type == PermissionsResolvedClaimType))
        {
            return Task.FromResult(principal);
        }

        var roleNames = principal.Claims.Where(c => RoleClaimTypes.Contains(c.Type)).Select(c => c.Value).ToList();
        var permissions = roleNames.SelectMany(RolePermissions.For).Distinct().ToList();

        foreach (var permission in permissions)
        {
            identity.AddClaim(new Claim(PermissionClaimType, permission));
        }

        identity.AddClaim(new Claim(PermissionsResolvedClaimType, "true"));

        return Task.FromResult(principal);
    }
}
