using System.Security.Claims;
using FastEndpoints;
using GoFit.Api.Endpoints.Account.Validators;
using GoFit.Api.Extensions;
using GoFit.Application.Interfaces.Services;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Account;

public class UserInfoEndpoint :
    EndpointWithoutRequest<LoggedUserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public SignInManager<AppUser> SignInManager { get; set; } = default!;
    
    public UserInfoEndpoint(
        UserManager<AppUser> userManager, 
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Get("Account/User-Info");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            await SendNoContentAsync(ct);
            return;
        }
        
        AppUser? user = await _userManager.GetUser(User);
            
        var loggedUser = new LoggedUserResponse(user.DisplayName, null);

        await SendOkAsync(loggedUser, ct);
    }
}

