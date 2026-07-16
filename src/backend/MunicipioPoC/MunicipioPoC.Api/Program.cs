using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MunicipioPoC.Core;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults and telemetry (Aspire)
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints(); // Aspire telemetry and health checks

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// In-memory or simulated database sync times (simulating historical data synchronization)
var syncTracker = new Dictionary<string, (DateTime LastSync, TimeSpan Sla)>
{
    { "Source_A", (DateTime.UtcNow.AddMinutes(-2), TimeSpan.FromMinutes(10)) },
    { "Source_B", (DateTime.UtcNow.AddMinutes(-5), TimeSpan.FromMinutes(20)) },
    { "Source_C", (DateTime.UtcNow.AddMinutes(-12), TimeSpan.FromMinutes(30)) },
    { "Source_D", (DateTime.UtcNow.AddMinutes(-32), TimeSpan.FromMinutes(40)) }
};

app.MapGet("/api/freshness", () =>
{
    var scores = new List<FreshnessScore>();
    foreach (var source in syncTracker)
    {
        var score = FreshnessCalculator.Calculate(source.Key, source.Value.LastSync, source.Value.Sla);
        scores.Add(score);
    }
    return Results.Ok(scores);
})
.WithName("GetFreshnessScores");

// Endpoint to simulate a refresh/sync of a source
app.MapPost("/api/sync/{sourceName}", (string sourceName) =>
{
    if (syncTracker.ContainsKey(sourceName))
    {
        var limit = syncTracker[sourceName].Sla;
        syncTracker[sourceName] = (DateTime.UtcNow, limit);
        return Results.Ok(new { Message = $"{sourceName} synchronized successfully." });
    }
    return Results.NotFound(new { Message = $"Source {sourceName} not found." });
})
.WithName("SyncSource");

app.Run();
