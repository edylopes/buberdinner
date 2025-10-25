using AspNetCoreRateLimit;
using BuberDinner.Api;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Filters;
using BuberDinner.Infrastructure.Authentication;
using BuberDinner.Infrastructure.Persistence;
using BuberDinner.Infrastructure.Persistence.Context;
using BuberDinner.Infrastructure.Services.SMTP;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


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
