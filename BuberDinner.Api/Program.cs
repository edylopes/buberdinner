using AspNetCoreRateLimit;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Filters;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
{
    builder
        .Services.AddInfraStructureModule(builder.Configuration);

    builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
    //options => options.Filters.Add<ErrorHandlingFilterAttribute>()
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
app.UseAuthorization();
app.MapControllers();
app.Run();
