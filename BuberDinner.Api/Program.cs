using AspNetCoreRateLimit;
using BuberDinner.Api;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Filters;
using BuberDinner.Infrastructure.Authentication;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddInfraStructureModule(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);


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
// builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();
app.UseIpRateLimiting();


if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuberDinner API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz del sitio
    });
}

app.UseHttpsRedirection();
// middleware auth
app.UseAuthentication();
app.UseAuthorization();
// middleware endpoints
app.MapControllers();
app.Run();
