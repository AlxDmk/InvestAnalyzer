using TinkoffInvest.Core.Models;

namespace TinkoffInvest.Core.Abstracts;

public interface IRepository<T> where T : class
{
    Task<Security?> GetByFigiAsync(string figi, CancellationToken cancellationToken);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);
}