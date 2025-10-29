using AspNetCoreRateLimit;

using BuberDinner.Api;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Filters;
using BuberDinner.Infrastructure.Persistence;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    //remove the server header
    builder.WebHost.UseKestrel(options => options.AddServerHeader = false);
    //Add Modules
    builder.Services.AddInfraStructureModule(builder.Configuration);
    builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
    //Aplly Migrations
    builder.Services.AddHostedService<MigrationHostedService>();
    //Background Worker | SMTP
    // builder.Services.AddHostedService<EmailBackgroundService>();
}

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BuberDinner API",
        Version = "v1",
        Description = "BuberDinner API",
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();


var app = builder.Build();

app.UseApiConfigurations();

app.UseIpRateLimiting();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuberDinner API v1");
        c.RoutePrefix = string.Empty; // Swagger on the root 
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapHealthChecks("/Health");

app.MapGet("/Health", () => Results.Ok("API It's Okay"))
  .WithTags("HealthCheck");


app.Run();
