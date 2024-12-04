using MediatR;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Core.Abstracts;
using TinkoffInvest.Core.Models;

namespace TinkoffInvest.Application.CQRS;

public  record GetAllLastPriceSubscribedRequest() : IRequest<List<Security?>>;

public class GetAllLastPriceSubscribedHandler(
    ISecuritiesRepository data,
    ILogger<GetAllLastPriceSubscribedHandler> logger
)
    : IRequestHandler<GetAllLastPriceSubscribedRequest, List<Security?>>
{
    private readonly ILogger<GetAllLastPriceSubscribedHandler> _logger = logger;

    public async Task<List<Security>?> Handle(GetAllLastPriceSubscribedRequest request,
        CancellationToken cancellationToken = default)
        => await data.GetAllSLastPriceSubscribedAsync(cancellationToken);
}