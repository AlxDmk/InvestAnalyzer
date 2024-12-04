using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TinkoffInvest.Application.CQRS;
using TinkoffInvest.Presentation.BackgroundServices;

namespace TinkoffInvest.Presentation.Controllers;

[ApiController]
[Route("api/securities")]
public class SecuritiesController(
    IMediator mediator,
    ILogger<SecuritiesController> logger,
    CancellationToken token = default) : ControllerBase
{

    [HttpGet("all")]
    public async Task<IActionResult> GetAllSecurities() =>
        Ok(await mediator.Send(new GetAllSecuritiesQuery(), token));
    

    [HttpGet("byFigi")]
    public async Task<IActionResult> GetByFigiAsync([FromQuery] string figi) =>
        Ok(await mediator.Send(new GetByFigiSecurityQuery(figi), token));
   
    
    [HttpGet]
    public async Task<IActionResult> GetAllSLastPriceSubscribedAsync()
        => Ok(await mediator.Send(new GetAllLastPriceSubscribedRequest()));

    [HttpGet("isSubscribed")]
    public async Task<IActionResult> IsLastPriceSubscribedAsync([FromQuery] string figi)
        => Ok(await mediator.Send(new IsLastPriceSubscribedQuery(figi)));


    [HttpPost("setAsSubscribed")]
    public async Task<IActionResult> SetAsLastPriceSubscribedAsync([FromBody] string figi)
    {
        await mediator.Send(new SetAsLastPriceSubscribedCommand(figi));
        var worker = HttpContext.RequestServices.GetService<RealTimeStream>();
        worker?.AddToSubscribed(figi, token);
        return Ok();
    }

    [HttpPost("UnsetAsSubscribed")]
    public async Task<IActionResult> UnsetAsLastPriceSubscribedAsync([FromBody] string figi)
    {
        await mediator.Send(new UnsetLastPriceSubscriptionCommand(figi));
        var worker = HttpContext.RequestServices.GetService<RealTimeStream>();
        worker?.RemoveFromSubscribed(figi, token);
       
        return Ok();
    }
}