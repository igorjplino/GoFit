using FastEndpoints;
using GoFit.Api.Authorization;
using GoFit.Api.Endpoints.Account.Validators;

namespace GoFit.Api.Endpoints.Account;

public class LogoutEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("Account/Logout");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        HttpContext.Response.Cookies.Delete(AuthCookie.Name, AuthCookie.Options());
        await Send.NoContentAsync();
    }
}
