using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text;
using Tomsoft.BusinessLogic.BusinessLogic;
using Tomsoft.BusinessLogic.Configuration;
using Tomsoft.BusinessLogic.Services;

var services = new ServiceCollection();
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";


var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true);

if(environmentName == "Development")
{
    configurationBuilder.AddUserSecrets<Program>();
}
configurationBuilder.AddEnvironmentVariables();

var configuration = configurationBuilder.Build();
services.Configure<LuceedConfig>(configuration.GetSection(LuceedConfig.ConfigName));


services.AddHttpClient("luceed", (serviceProvider, client) =>
{
    var luceedConfig = serviceProvider.GetRequiredService<IOptions<LuceedConfig>>()?.Value;
    if(luceedConfig is null)
    {
        throw new ArgumentException("Luceed Config is missing");
    }


    client.BaseAddress = new Uri(luceedConfig.BaseUrl);
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", luceedConfig.GetValueForBAsicAuthHeader());
});

services.AddTransient<LuceedApiClient>();
services.AddMediatR(typeof(Program));

var serviceProvider = services.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var mediaror = scope.ServiceProvider.GetRequiredService<IMediator>();


    //var response = await mediaror.Send(new SearchProductRequest()
    //{
    //    Query = String.Empty
    //});

    //var response = await mediaror.Send(new GetPaymentTypeForProductRequest()
    //{
    //    ProductId = "4986-1",
    //    StartDate = DateTime.UtcNow.AddYears(-23)
    //});

    var response = await mediaror.Send(new GetTransactionsForProductRequest()
    {
        ProductId = "4986-1",
        StartDate = DateTime.UtcNow.AddYears(-23)
    });
}
