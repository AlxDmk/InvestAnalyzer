using MediatR;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Core.Abstracts;

namespace TinkoffInvest.Application.CQRS;

public record SetAsLastPriceSubscribedCommand(string Figi) : IRequest<bool>;

public class SetAsLastPriceSubscribedHandler(
    ISecuritiesRepository data,
    ILogger<SetAsLastPriceSubscribedCommand> logger)
    : IRequestHandler<SetAsLastPriceSubscribedCommand, bool>
{
    private readonly ILogger<SetAsLastPriceSubscribedCommand> _logger = logger;

    public async Task<bool> Handle(SetAsLastPriceSubscribedCommand request, CancellationToken cancellationToken)
        => await data.SetAsLastPriceSubscribedAsync(request.Figi, cancellationToken);
}