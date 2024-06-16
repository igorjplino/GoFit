using FastEndpoints.Security;

namespace GoFit.Api.Extentions;

public static class IdentityServiceExtention
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
