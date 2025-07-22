using BuberDinner.Api.Errors;
using BuberDinner.Api.Errors;
using BuberDinner.Api.Filters;
using BuberDinner.Application;
using BuberDinner.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication().AddInrastructure(builder.Configuration);

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
