using FastEndpoints;
using FastEndpoints.Swagger;
using GoFit.Api.Extensions;
using GoFit.Api.GlobalProcessors.Pre;
using GoFit.Application;
using GoFit.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddFastEndpoints();

builder.Services.SwaggerDocument(o =>
{
    o.EnableJWTBearerAuth = false;
    o.DocumentSettings = s =>
    {
        s.Title= "API GoFit";
        s.Version= "v1";
    };
});

// Services containers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

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
    app.UseSwaggerGen();
}

app.Run();
