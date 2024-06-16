using FastEndpoints;
using GoFit.Domain.Entities;
using GoFit.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Account;

public class LoginEndpoint :
    BaseEndpoint<LoginRequest, LoggedUserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    public SignInManager<IdentityUser> _signInManager;

    public LoginEndpoint(
        ILogger<LoginEndpoint> logger,
        UserManager<AppUser> userManager,
        SignInManager<IdentityUser> signInManager)
        : base(logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post("Account/Login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);

        if (!result.Succeeded)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var loggedUser = new LoggedUserResponse(user.DisplayName, "token here");

        await SendOkAsync(loggedUser, ct);
    }
}

public record LoginRequest(
    string Email,
    string Password)
{ }

public record LoggedUserResponse(
    string DisplayName,
    string Token)
{ }
