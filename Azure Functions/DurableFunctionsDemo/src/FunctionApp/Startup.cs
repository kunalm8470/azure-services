using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SharedLibs.Contracts;
using SharedLibs.Services;
using System;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]
namespace FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string jsonTypicodeHost = Environment.GetEnvironmentVariable("JSON_TYPICODE_HOST");
            HttpClient client = new();
            client.BaseAddress = new Uri(jsonTypicodeHost);

            builder.Services.AddSingleton<IUserService, UserService>((implementationFactory) =>
            {
                return new UserService(client);
            });

            builder.Services.AddSingleton<IUserPostService, UserPostService>((implementationFactory) =>
            {
                return new UserPostService(client);
            });

            builder.Services.AddSingleton<IUserPostCommentService, UserPostCommentService>((implementationFactory) =>
            {
                return new UserPostCommentService(client);
            });

            builder.Services.AddLogging();
        }
    }
}