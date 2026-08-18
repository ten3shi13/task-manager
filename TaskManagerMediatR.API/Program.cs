using Microsoft.OpenApi;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerUI;
using TaskManagerMediatR.Application;
using TaskManagerMediatR.Infrastructure;
using TaskManagerMediatR.Infrastructure.BackgroundJobs;

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
builder.Services.AddApplication();

builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

    configure.AddJob<ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey));

    configure.AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(100)
                        .RepeatForever()));
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

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

//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider
//        .GetRequiredService<TaskManagerMediatRDbContext>();

//    await dbContext.Database.MigrateAsync();

//    var seeder = scope.ServiceProvider
//        .GetRequiredService<IDatabaseSeeder>();

//    await seeder.SeedAsync();
//}

app.Run();

