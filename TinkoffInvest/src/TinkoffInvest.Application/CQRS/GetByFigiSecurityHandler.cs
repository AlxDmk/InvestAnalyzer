using MediatR;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Core.Abstracts;
using TinkoffInvest.Core.Models;

namespace TinkoffInvest.Application.CQRS;

public record GetByFigiSecurityQuery(string Figi) : IRequest<Security?>;

public class GetByFigiSecurityHandler(ISecuritiesRepository data, ILogger<GetByFigiSecurityHandler> logger)
    : IRequestHandler<GetByFigiSecurityQuery, Security?>
{
    private readonly ILogger<GetByFigiSecurityHandler> _logger = logger;

    public async Task<Security?> Handle(GetByFigiSecurityQuery request, CancellationToken cancellationToken) =>
        await  data.GetByFigiAsync(request.Figi, cancellationToken);
}