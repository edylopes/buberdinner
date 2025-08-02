using BuberDinner.Api.Errors;
using BurberDinner.Application;
using BurberDinner.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
.AddInfrastructure(
    builder.Configuration);

builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();

//options => options.Filters.Add<ErrorHandlingFilterAttribute>()
builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();