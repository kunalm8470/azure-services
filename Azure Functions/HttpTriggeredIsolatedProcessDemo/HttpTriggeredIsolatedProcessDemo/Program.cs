using HttpTriggeredIsolatedProcessDemo.Middlewares;
using HttpTriggeredIsolatedProcessDemo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults((builder) =>
    {
        HttpClient httpClient = new();
        httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

        builder.Services.AddSingleton(new JsonTypicodeService(httpClient));

        builder.UseMiddleware<UnhandledExceptionMiddleware>();
    })
    .Build();

host.Run();