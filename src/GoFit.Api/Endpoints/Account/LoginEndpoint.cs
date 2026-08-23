using GoFit.Api.Endpoints.Account.Validators;
using GoFit.Api.Extensions;
using GoFit.Application.Interfaces.Services;
using GoFit.Domain.Authorization;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Account;

public class LoginEndpoint :
    BaseEndpoint<LoginRequest, LoggedUserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public SignInManager<AppUser> SignInManager { get; set; } = default!;
    public LoginEndpoint(
        ILogger<LoginEndpoint> logger,
        UserManager<AppUser> userManager, 
        IAuthorizationService authorizationService)
        : base(logger)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    public override void Configure()
    {
        Post("Account/Login");
        AllowAnonymous();
        Validator<LoginRequestValidator>();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        AppUser? user = await _userManager.FindByEmailAsync(req.Email);

        if (user is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var result = await SignInManager.CheckPasswordSignInAsync(user, req.Password, false);

        if (!result.Succeeded)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _authorizationService.GenerateToken(user, roles);

        HttpContext.Response.Cookies.Append("access_token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1000)
        });

        var role = roles.FirstOrDefault() ?? string.Empty;
        var loggedUser = new LoggedUserResponse(user.DisplayName, user.Email, accessToken, role, RolePermissions.For(role).ToArray());

        await Send.OkAsync(loggedUser, ct);
    }
}

public record LoginRequest(
    string Email,
    string Password)
{ }

public record LoggedUserResponse(
    string DisplayName,
    string? Email,
    string? Token,
    string Role,
    string[] Permissions)
{ }
