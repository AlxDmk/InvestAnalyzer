using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinkoffInvest.Application;

namespace TinkoffInvest.Presentation.Configuration;

public class ApplicationServicesInstaller:IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region MEDIATR

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblyContaining(typeof(TinkoffInvestAppEntrypoint));
        });

        //TODO - Настроить конвеер ошибок Mediatr
//services.AddTransient(typeof(IPipelineBehavior<,>, typeof(ExceptionHandlingBehavior<,>));

        #endregion
    }
}