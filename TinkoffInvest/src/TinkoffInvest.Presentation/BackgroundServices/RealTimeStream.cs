using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tinkoff.InvestApi.V1;
using TinkoffInvest.Application.CQRS;
using TinkoffInvest.Core.TinkoffApiClient;
using TinkoffInvest.Presentation.Hubs;

namespace TinkoffInvest.Presentation.BackgroundServices;

internal sealed class RealTimeStream : BackgroundService
{
    private readonly TinkoffInvestGrpcApiClient _client;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RealTimeStream> _logger;
    private readonly IHubContext<StockFeedHub, IStockUpdateClient> _hubContext;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RealTimeStream(TinkoffInvestGrpcApiClient client,
        IConfiguration configuration,
        ILogger<RealTimeStream> logger,
        IHubContext<StockFeedHub, IStockUpdateClient> hubContext,
        IServiceScopeFactory serviceScopeFactory)
    {
        _client = client;
        _configuration = configuration;
        _logger = logger;
        _hubContext = hubContext;
        _serviceScopeFactory = serviceScopeFactory;
    }
    private List<LastPriceInstrument> LastPriceInstruments { get; set; } = [];

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        LastPriceInstruments = [];
        _logger.LogInformation("Сервер запускается");

        
       
        _logger.LogInformation($"{_configuration.GetSection("ApplicationSettings:Initial").Value}");
        
        // if (File.Exists($"{Directory.GetParent(Directory.GetCurrentDirectory())}/src/TinkoffInvest/src/TinkoffInvest.Data/shares.json"))
        // {
        //     
        // }
        _logger.LogWarning(File.Exists($"{Directory.GetParent(Directory.GetCurrentDirectory())}/src/TinkoffInvest/src/TinkoffInvest.Data/shares.json") ? "Файл существует" : " Файл не найден");
        
        
        return base.StartAsync(cancellationToken);
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitializeLastPriceSubscribeListAsync(stoppingToken);

        var stream =
            _client.MarketDataStream.MarketDataStream(cancellationToken: stoppingToken);

        await stream.RequestStream.WriteAsync(new MarketDataRequest
        {
            SubscribeLastPriceRequest = new SubscribeLastPriceRequest()
            {
                Instruments = { LastPriceInstruments },
                SubscriptionAction = SubscriptionAction.Subscribe
            }
        }, stoppingToken);

        await foreach (var response in stream.ResponseStream.ReadAllAsync(cancellationToken: stoppingToken))
        {
            if (response.LastPrice == null) continue;
            var price = response.LastPrice!.Price.Units +
                        (decimal)response.LastPrice.Price.Nano / 1000000000;
            await _hubContext.Clients.Group(response.LastPrice.Figi)
                .ReceiveStockPriceUpdate(new StockPriceUpdate(response.LastPrice?.Figi!, price));

            Console.WriteLine($"{response.LastPrice?.Figi} ---> {price}");
        }
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Сервер выключается");
        await base.StopAsync(cancellationToken);
    }
    
    private async Task InitializeLastPriceSubscribeListAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var list = await mediator.Send(new GetAllLastPriceSubscribedRequest(), stoppingToken);
        if (list.Count == 0)
        {
            _logger.LogWarning("Список мониторинга цен в реальном времени пуст");
        }
        foreach (var share in list)
        {
            LastPriceInstruments.Add(new LastPriceInstrument { InstrumentId = share?.Figi });
            _logger.LogInformation($" Initially" + $"  {share?.Figi}  securities subscribed");
        }
    }
    
    public async Task AddToSubscribed(string figi, CancellationToken token)
    {
        LastPriceInstruments.Add(new LastPriceInstrument { InstrumentId = figi });
        _logger.LogInformation("Новый подписант добавлен");
        var stream = _client.MarketDataStream.MarketDataStream(cancellationToken: token);
        await stream.RequestStream.WriteAsync(new MarketDataRequest
        {
            SubscribeLastPriceRequest = new SubscribeLastPriceRequest
            {
                SubscriptionAction = SubscriptionAction.Subscribe,
                Instruments = { new []{new LastPriceInstrument{InstrumentId = figi}} }
            }
        }, token);
    }
    
    public async Task RemoveFromSubscribed(string figi, CancellationToken token)
    {
        var r = LastPriceInstruments.FirstOrDefault(s => s.InstrumentId == figi);
        if (r is null) return ;

        LastPriceInstruments.Remove(r);
        var stream = _client.MarketDataStream.MarketDataStream(cancellationToken: token);
        await stream.RequestStream.WriteAsync(new MarketDataRequest
        {
            SubscribeLastPriceRequest = new SubscribeLastPriceRequest
            {
                SubscriptionAction = SubscriptionAction.Unsubscribe,
                Instruments = { new []{new LastPriceInstrument{InstrumentId = figi}} }
            }
        }, token);
    }
}