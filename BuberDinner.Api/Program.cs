using BuberDinner.Api.Errors;
using BuberDinner.Api.Filters;
using BurberDinner.Application;
using BurberDinner.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();

// Register the error handling filter globally
// ErrorMappingRegistry.RegisterDefaults();

builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());

var app = builder.Build();

// app.UseMiddleware<ErrorHandlingMiddleware>();

// app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
