using HttpTriggerFunctionDemo.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(HttpTriggerFunctionDemo.Startup))]
namespace HttpTriggerFunctionDemo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<JsonTypicodeService>((client) =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            });
        }
    }
}
