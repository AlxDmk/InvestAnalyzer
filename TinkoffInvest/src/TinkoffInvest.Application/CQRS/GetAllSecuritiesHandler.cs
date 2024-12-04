using MediatR;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Core.Abstracts;
using TinkoffInvest.Core.Models;

namespace TinkoffInvest.Application.CQRS;

public record GetAllSecuritiesQuery : IRequest<IReadOnlyList<Security>>;

public class GetAllSecuritiesHandler(ISecuritiesRepository data, ILogger<GetAllSecuritiesHandler> logger)
    : IRequestHandler<GetAllSecuritiesQuery, IReadOnlyList<Security>>
{
    private readonly ISecuritiesRepository _data = data;
    private readonly ILogger<GetAllSecuritiesHandler> _logger = logger;

    public async Task<IReadOnlyList<Security>> Handle(GetAllSecuritiesQuery request, CancellationToken cancellationToken) =>
        await _data.GetAllAsync(cancellationToken);
}