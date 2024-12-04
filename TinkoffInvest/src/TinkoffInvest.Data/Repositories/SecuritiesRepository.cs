using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Core.Abstracts;
using TinkoffInvest.Core.Models;
using TinkoffInvest.Data.Entities;

namespace TinkoffInvest.Data.Repositories;

public class SecuritiesRepository : ISecuritiesRepository
{
    private readonly TinkoffInvestDbContext _context;
    private readonly ILogger<SecuritiesRepository> _logger;
    private readonly IMapper _mapper;

    public SecuritiesRepository(TinkoffInvestDbContext context,
        ILogger<SecuritiesRepository> logger,
        IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Security?> GetByFigiAsync(string figi, CancellationToken cancellationToken)
    {
        var result =
            await _context.Set<SecurityEntity>().AsNoTracking()
                .FirstOrDefaultAsync(s => s.Figi == figi, cancellationToken);
        return _mapper.Map<Security>(result) ?? null;
    }

    public async Task<IReadOnlyList<Security>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _context.Set<SecurityEntity>().AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        return _mapper.Map<List<Security>>(result);
    }

    public async Task<bool> IsLastPriceSubscribedAsync(string figi, CancellationToken token = default) =>
        await _context.Set<SecurityEntity>().AsNoTracking()
            .AnyAsync(s => s.Figi == figi && s.isLastPriceSubscribed == true, token);

    public async Task<List<Security>?> GetAllSLastPriceSubscribedAsync(CancellationToken token)
    {
        var result = await _context.Set<SecurityEntity>().AsNoTracking().Where(s => s.isLastPriceSubscribed)
            .ToListAsync(token);
        return _mapper.Map<List<Security>>(result) ?? null;
    }

    public async Task<bool> SetAsLastPriceSubscribedAsync(string figi, CancellationToken token)
    {
        var s = await _context.Set<SecurityEntity>().FirstOrDefaultAsync(s => s.Figi == figi, cancellationToken: token);
        if (s == null) return false;

        s.isLastPriceSubscribed = true;
        await _context.SaveChangesAsync(token);
        _logger.LogInformation($"Security with Fifi {s.Figi} is subscribed to LastPrice Monitoring");

        return true;
    }

    public async Task<bool> UnsetAsLastPriceSubscribedAsync(string figi, CancellationToken token)
    {
        var s = await _context.Set<SecurityEntity>()
            .FirstOrDefaultAsync(s => s.Figi == figi && s.isLastPriceSubscribed == true, token);
        if (s is null) return false;

        s.isLastPriceSubscribed = false;
        await _context.SaveChangesAsync(token);
        _logger.LogInformation($"Security with Fifi {s.Figi} is unsubscribed from LastPrice Monitoring");
        return true;
    }
}