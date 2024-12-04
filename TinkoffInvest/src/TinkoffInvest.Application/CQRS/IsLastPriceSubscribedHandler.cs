using MediatR;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Core.Abstracts;

namespace TinkoffInvest.Application.CQRS;

public record IsLastPriceSubscribedQuery(string Figi) : IRequest<bool>;

public class IsLastPriceSubscribedHandler(
    ISecuritiesRepository data,
    ILogger<IsLastPriceSubscribedHandler> logger,
    CancellationToken cancellationToken = default)
    : IRequestHandler<IsLastPriceSubscribedQuery, bool>
{
    private readonly ISecuritiesRepository _data = data;
    private readonly ILogger<IsLastPriceSubscribedHandler> _logger = logger;
    private readonly CancellationToken _cancellationToken = cancellationToken;

    public async Task<bool> Handle(IsLastPriceSubscribedQuery request, CancellationToken cancellationToken)
        => await _data.IsLastPriceSubscribedAsync(request.Figi, cancellationToken);
}