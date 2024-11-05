using FastEndpoints.Security;

namespace GoFit.Api.Extensions;

public static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services
            .AddAuthenticationJwtBearer(x => x.SigningKey = "")
            .AddAuthentication();

        return services;
    }
}
