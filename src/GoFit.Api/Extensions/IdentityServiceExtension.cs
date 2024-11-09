using FastEndpoints.Security;
using GoFit.Domain.Entities.Identity;
using GoFit.Infrastructure.Contexts.IdentityDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Api.Extensions;

public static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthenticationJwtBearer(x => x.SigningKey = config["Token:Key"]);
        
        services.AddDefaultIdentity<AppUser>(opt =>
        {
            // add identity options here, if necessary
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddSignInManager();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });
        services.AddAuthorization();
        
        return services;
    }
}