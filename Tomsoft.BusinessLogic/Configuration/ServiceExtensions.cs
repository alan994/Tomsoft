using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomsoft.BusinessLogic.Services;

namespace Tomsoft.BusinessLogic.Configuration
{
    public static class ServiceExtensions
    {      
        public static IServiceCollection AddLuceedApiClient(this IServiceCollection services)
        {
            services.AddTransient<LuceedApiClient>();
            return services;
        }

        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceExtensions));
            services.AddHttpClient("luceed", (serviceProvider, client) =>
            {
                var luceedConfig = serviceProvider.GetRequiredService<IOptions<LuceedConfig>>()?.Value;
                if (luceedConfig is null)
                {
                    throw new ArgumentException("Luceed Config is missing");
                }


                client.BaseAddress = new Uri(luceedConfig.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", luceedConfig.GetValueForBAsicAuthHeader());
            })
            .AddPolicyHandler(GetRetryPolicy());
            return services;
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

    }
}
