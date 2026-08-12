using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerUI;
using TaskManagerMediatR.Infrastructure;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Manager API RESTful API",
        Version = "v1",
        Description = "Task Manager API RESTful API Doc.",
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API v1");
        s.DocumentTitle = "Task Manager API";
        s.RoutePrefix = "";
        s.DefaultModelRendering(ModelRendering.Example);
        s.DefaultModelExpandDepth(2);
        s.DisplayRequestDuration();

    });
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<TaskManagerMediatRDbContext>();

    await dbContext.Database.MigrateAsync();

    var seeder = scope.ServiceProvider
        .GetRequiredService<IDatabaseSeeder>();

    await seeder.SeedAsync();
}

app.Run();

