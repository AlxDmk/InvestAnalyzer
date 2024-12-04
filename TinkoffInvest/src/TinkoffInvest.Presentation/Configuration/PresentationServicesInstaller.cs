using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinkoffInvest.Presentation.BackgroundServices;

namespace TinkoffInvest.Presentation.Configuration;

public class PresentationServicesInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
            
        services.AddCors(options =>
        {
            options.AddPolicy("React", policyBuilder =>
            {
                policyBuilder.WithOrigins("http://localhost:5173", "http://localhost:5174")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.AddHostedService<RealTimeStream>();
    }
}