using AspNetCoreRateLimit;

using BuberDinner.Api;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Filters;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Beahviors.Logger;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Infrastructure.Persistence;

using Microsoft.OpenApi.Models;

using OneOf;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddInfraStructureModule(builder.Configuration);

    builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
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

//Aplly MIGRATIONS
builder.Services.AddHostedService<MigrationHostedService>();

// builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;

    var strategie = provider.GetRequiredService<ILoggingStrategy<RegisterCommand, OneOf<AuthenticationResult, AppError>>>();

    Console.WriteLine("📦Strategie encontrada:");
    Console.WriteLine($"➡️ {strategie.GetType().Name}");
}


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
// middleware auth
app.UseAuthentication();
app.UseAuthorization();
// middleware endpoints
app.MapControllers();


app.Run();
