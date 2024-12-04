using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinkoffInvest.Core.TinkoffApiClient;

namespace TinkoffInvest.Presentation.Configuration;

public static class TinkoffInvestGrpcClientService
{
    public static IServiceCollection AddTinkoffInvestGrpcApiClient(this IServiceCollection services,
        Action<IServiceProvider, TinkoffInvestGrpcApiClientSetting> configServices)
    {
        services.AddGrpcClient<TinkoffInvestGrpcApiClient>((provider, options) =>
        {
            var settings = new TinkoffInvestGrpcApiClientSetting();
            configServices(provider, settings);
            options.Address = settings.TrainingMode
                ? new Uri("https://sandbox-invest-public-api.tinkoff.ru:443")
                : new Uri("https://invest-public-api.tinkoff.ru:443");
        }).ConfigureChannel((provider, options) =>
        {
            var settings = new TinkoffInvestGrpcApiClientSetting();
            configServices(provider, settings);
            var token = settings.Token ?? throw new Exception("Token is required!");

            var credentials = CallCredentials.FromInterceptor((_, metadata) =>
            {
                metadata.Add("Authorization", $"Bearer {token}");
                return Task.CompletedTask;
            });
            options.Credentials = ChannelCredentials.Create(new SslCredentials(), credentials);

            options.ServiceConfig = new ServiceConfig
            {
                MethodConfigs =
                {
                    new MethodConfig
                    {
                        RetryPolicy = new RetryPolicy
                        {
                            MaxAttempts = 5,
                            InitialBackoff = TimeSpan.FromMicroseconds(1),
                            MaxBackoff = TimeSpan.FromMicroseconds(5),
                            BackoffMultiplier = 1.2,
                            RetryableStatusCodes = { StatusCode.Unavailable }
                        }
                    }
                }
            };
        });
        return services;
    }
}