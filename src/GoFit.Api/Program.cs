using FastEndpoints;
using GoFit.Api.Extensions;
using GoFit.Api.GlobalProcessors.Pre;
using GoFit.Application;
using GoFit.Domain.Entities.Identity;
using GoFit.Hangfire;
using GoFit.Hangfire.Recurring;
using GoFit.Infrastructure;
using GoFit.Infrastructure.Contexts.IdentityDb;
using Hangfire;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.AddCors();

// Services containers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddHangfireServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    await identityContext.ApplyMigrationAsync();

    await AppIdentityDbContextInitialise.EnsureRolesAsync(services.GetRequiredService<RoleManager<IdentityRole>>());

    var identitySeedLogger = services.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeed");
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    await AppIdentityDbContextInitialise.EnsureAdminAsync(userManager, app.Configuration, identitySeedLogger);
    await AppIdentityDbContextInitialise.BackfillMissingRolesAsync(userManager, identitySeedLogger);
}

// Configure the HTTP request pipeline.

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:4200","https://localhost:4200"));

app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(x =>
    {
        x.Endpoints.RoutePrefix = "api";
        x.Endpoints.Configurator = ep =>
        {
            ep.PreProcessor<CollectMetricPreProcessor>(Order.Before);
            ep.PostProcessor<CollectMetricPostProcessor>(Order.After);
        };
    });

if (app.Environment.IsDevelopment())
{

}

app.UseHangfireDashboard();
app.RegisterAllRecurringJob();

app.Run();
