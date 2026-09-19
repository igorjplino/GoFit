namespace GoFit.Api.Authorization;

/// <summary>
/// The cookie that carries the JWT. Defined once so the login that writes it, the logout that deletes it and
/// the bearer handler that reads it can never disagree: a delete with different attributes (e.g. Path) leaves
/// the browser's cookie in place.
/// </summary>
public static class AuthCookie
{
    public const string Name = "access_token";

    public static CookieOptions Options(DateTimeOffset? expires = null)
        => new()
        {
            HttpOnly = true,
            // Always Secure: the API and both Angular dev servers already run over https.
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = expires
        };
}
