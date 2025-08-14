
using AspNetCoreRateLimit;
using BuberDinner.Api;
using BuberDinner.Api.Common.Errors;
using BurberDinner.Application;
using BurberDinner.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.
         AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPolicy(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
    //options => options.Filters.Add<ErrorHandlingFilterAttribute>()

}

var app = builder.Build();
app.UseIpRateLimiting();

//app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();