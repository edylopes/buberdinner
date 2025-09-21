using AspNetCoreRateLimit;
using BuberDinner.Api;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Common.Mapping;
using BuberDinner.Api.Filters;

using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder
        .Services.AddInfraStructureModule(builder.Configuration);
    
    builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
    //options => options.Filters.Add<ErrorHandlingFilterAttribute>()
}

var app = builder.Build();
app.UseIpRateLimiting();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
