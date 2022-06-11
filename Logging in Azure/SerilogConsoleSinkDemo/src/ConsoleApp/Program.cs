using Serilog;
using Serilog.Formatting.Json;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.WithThreadId()
    .Enrich.WithProcessId()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithEnvironmentUserName()
    .WriteTo.Async(wt => wt.Console(formatter: new JsonFormatter(renderMessage: true)))
    .CreateLogger();

try
{
    Log.Information("Hello world, {0}", new { prop1 = 1, prop2 = 2 });
    Log.Information("Hello world2");

    throw new Exception("testing");
}
catch(Exception ex)
{
    Log.Warning(ex, "Failed because: {0}", ex.Message);
}


Log.CloseAndFlush();
