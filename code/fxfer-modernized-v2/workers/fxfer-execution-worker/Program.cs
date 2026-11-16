using Fxfer.ExecutionWorker.Host.Composition;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFxferExecutionWorker(builder.Configuration, builder.Environment);

var app = builder.Build();

Fxfer.ExecutionWorker.Batch.Business.BatchServiceLocator.ServiceProvider = app.Services;

app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live"),
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready"),
});

app.Run();

public partial class Program;
