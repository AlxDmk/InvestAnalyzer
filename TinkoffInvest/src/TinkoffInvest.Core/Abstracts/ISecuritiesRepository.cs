using TinkoffInvest.Core.Models;

namespace TinkoffInvest.Core.Abstracts;

public interface ISecuritiesRepository : IRepository<Security>
{
    Task<bool> IsLastPriceSubscribedAsync(string figi, CancellationToken token);
    Task<List<Security>?> GetAllSLastPriceSubscribedAsync(CancellationToken token);
    Task<bool> SetAsLastPriceSubscribedAsync(string figi, CancellationToken token);
    Task<bool> UnsetAsLastPriceSubscribedAsync(string figi, CancellationToken token);
}