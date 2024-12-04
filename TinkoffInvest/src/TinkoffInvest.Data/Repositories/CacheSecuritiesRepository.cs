using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Core.Abstracts;
using TinkoffInvest.Core.Models;

namespace TinkoffInvest.Data.Repositories;

public class CacheSecuritiesRepository(
    ISecuritiesRepository repository,
    ILogger<CacheSecuritiesRepository> logger,
    IMemoryCache memoryCache) : ISecuritiesRepository
{
    private readonly ILogger<CacheSecuritiesRepository> _logger = logger;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ISecuritiesRepository _securitiesRepository = repository;

    public Task<Security?> GetByFigiAsync(string figi, CancellationToken cancellationToken)
    {
        string key = $"security-{figi}";
        return _memoryCache.GetOrCreateAsync(key, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromDays(7));
            return _securitiesRepository.GetByFigiAsync(figi, cancellationToken);
        });
    }

    public async Task<IReadOnlyList<Security>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    public async Task<bool> IsLastPriceSubscribedAsync(string figi, CancellationToken token)
    {
        return await repository.IsLastPriceSubscribedAsync(figi, token);
    }

    public async Task<List<Security>?> GetAllSLastPriceSubscribedAsync(CancellationToken token)
    {
        return await repository.GetAllSLastPriceSubscribedAsync(token);
    }

    public async Task<bool> SetAsLastPriceSubscribedAsync(string figi, CancellationToken token)
    {
        return await repository.SetAsLastPriceSubscribedAsync(figi, token);
    }

    public async Task<bool> UnsetAsLastPriceSubscribedAsync(string figi, CancellationToken token)
    {
        return await repository.UnsetAsLastPriceSubscribedAsync(figi, token);
    }
}