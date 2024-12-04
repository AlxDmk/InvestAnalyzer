using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TinkoffInvest.Presentation.Configuration;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}