using AspNetCoreRateLimit;
using BuberDinner.Api;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Common.Mapping;
using BuberDinner.Api.Filters;
using BurberDinner.Application;
using BurberDinner.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder
        .Services.AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPolicy(builder.Configuration)
        .AddMappings();
        
    builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
    //options => options.Filters.Add<ErrorHandlingFilterAttribute>()
}

var app = builder.Build();
app.UseIpRateLimiting();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
