using FluentValidation.Results;
using GoFit.Api.Endpoints.Account.Validators;
using GoFit.Api.Extensions;
using GoFit.Application.Interfaces.Services;
using GoFit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Endpoints.Account;

public class RegisterEndpoint :
    BaseEndpoint<RegisterRequest, RegistredResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public RegisterEndpoint(
        ILogger<RegisterEndpoint> logger,
        UserManager<AppUser> userManager,
        IAuthorizationService authorizationService)
        : base(logger)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    public override void Configure()
    {
        Post("Account/Register");
        AllowAnonymous();
        Validator<RegisterValidator>();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var user = new AppUser
        {
            DisplayName = req.Name,
            UserName = req.Name,
            Email = req.Email
        };
        
        var result = await _userManager.CreateAsync(user, req.Password);

        if (result.Succeeded)
        {
            var regitredUser = new RegistredResponse(
                user.DisplayName,
                user.Email,
                _authorizationService.GenerateToken(user));  
            
            await SendOkAsync(regitredUser, ct);
        }

        foreach (var error in result.Errors)
        {
            AddError(new ValidationFailure(error.Code, error.Description));
        }
        
        ThrowIfAnyErrors();
    }
}

public record RegisterRequest(
    string Name,
    string Email,
    string Password)
{ }

public record RegistredResponse(
    string DisplayName,
    string Email,
    string AccessToken)
{ }

