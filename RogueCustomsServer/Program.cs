using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Collections.Generic;
using RogueCustomsServer;
using System.Threading.Tasks;
using Roguelike.Controllers;
using Roguelike.Services;
using Microsoft.Extensions.Caching.Memory;
using System;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

builder.Logging.ClearProviders();
var path = config.GetValue<string>("Logging:FilePath").Replace("{DATE}", $"{DateTime.Now.ToString("s").Replace(":", "-")}");
var logger = new LoggerConfiguration()
    .WriteTo.File(path)
    .WriteTo.Console(Serilog.Events.LogEventLevel.Verbose)
    .WriteTo.Debug(Serilog.Events.LogEventLevel.Debug)
    .CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddSingleton<DungeonController>();
builder.Services.AddSingleton<DungeonService>();

// Use the Startup class as the entry point for configuring the application.
var startup = new Startup(config);

startup.ConfigureServices(builder.Services);

var app = builder.Build();
app.UseHttpLogging();

startup.Configure(app, app.Environment);

app.MapControllers();

app.Run();
