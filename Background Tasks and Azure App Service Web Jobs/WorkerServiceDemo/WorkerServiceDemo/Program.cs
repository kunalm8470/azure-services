using Serilog;
using Serilog.Events;
using WorkerServiceDemo;

string commonAppDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

// Create static logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(commonAppDirectoryPath, "logs", "server_log.log"), 
        restrictedToMinimumLevel: LogEventLevel.Information,
        rollingInterval: RollingInterval.Day
     )
    .CreateLogger();

try
{
    IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddScoped<IScopedOperation, ScopedOperation>();
    })
    .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
