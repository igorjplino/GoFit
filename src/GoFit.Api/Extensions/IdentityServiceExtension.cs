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
        services.AddAuthenticationJwtBearer(
        signingOptions =>
        {
            signingOptions.SigningKey = config["Token:Key"];
        },
        options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    if (ctx.Request.Cookies.ContainsKey("access_token"))
                    {
                        ctx.Token = ctx.Request.Cookies["access_token"];
                    }
                        
                    return Task.CompletedTask;
                }
            };
        });
        
        services.AddDefaultIdentity<AppUser>(opt =>
        {
            // add identity options here, if necessary
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddSignInManager();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        services.AddAuthorization();
        
        return services;
    }
}