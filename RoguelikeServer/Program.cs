using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

builder.Logging.ClearProviders();
var path = config.GetValue<string>("Logging:FilePath");
var logger = new LoggerConfiguration()
    .WriteTo.File(path)
    .WriteTo.Console(Serilog.Events.LogEventLevel.Verbose)
    .WriteTo.Debug(Serilog.Events.LogEventLevel.Debug)
    .CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

var app = builder.Build();
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
        {
            ["activated"] = false
        };
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
