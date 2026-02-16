using FastEndpoints;
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
        HttpContext.Response.Cookies.Delete("access_token");
        await Send.NoContentAsync();
    }
}
