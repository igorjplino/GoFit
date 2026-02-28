using FastEndpoints;
using GoFit.Api.Extensions;
using GoFit.Api.GlobalProcessors.Pre;
using GoFit.Application;
using GoFit.Hangfire;
using GoFit.Hangfire.Recurring;
using GoFit.Infrastructure;
using Hangfire;

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
