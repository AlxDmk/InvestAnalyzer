using MediatR;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Core.Abstracts;

namespace TinkoffInvest.Application.CQRS;

public record UnsetLastPriceSubscriptionCommand(string Figi) : IRequest<bool>;

public class UnsetLastPriceSubscriptionHandler(
    ISecuritiesRepository data,
    ILogger<UnsetLastPriceSubscriptionHandler> logger)
    : IRequestHandler<UnsetLastPriceSubscriptionCommand, bool>
{
    private readonly ISecuritiesRepository _data = data;
    private readonly ILogger<UnsetLastPriceSubscriptionHandler> _logger = logger;

    public async Task<bool> Handle(UnsetLastPriceSubscriptionCommand request, CancellationToken cancellationToken) =>
        await _data.UnsetAsLastPriceSubscribedAsync(request.Figi, cancellationToken);
}