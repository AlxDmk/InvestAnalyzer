using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinkoffInvest.Core.Abstracts;
using TinkoffInvest.Data;
using TinkoffInvest.Data.MapperProfiles;
using TinkoffInvest.Data.Repositories;

namespace TinkoffInvest.Presentation.Configuration;

public class DataServicesInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddScoped<ISecuritiesRepository,SecuritiesRepository>();
        services.Decorate<ISecuritiesRepository, CacheSecuritiesRepository>();
        
        var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new DataMapperProfile()));
        services.AddSingleton(mapperConfiguration.CreateMapper());
        
        services.AddDbContext<TinkoffInvestDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(TinkoffInvestDbContext)));
        });
    }
}