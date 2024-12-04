using Microsoft.AspNetCore.SignalR;

namespace TinkoffInvest.Presentation.Hubs;

public interface IStockUpdateClient
{
    Task ReceiveStockPriceUpdate(StockPriceUpdate update);
}

public sealed record StockPriceUpdate(string Ticker, decimal Price);

public sealed class StockFeedHub : Hub<IStockUpdateClient>
{
    public async Task JoinToTicker(string ticker) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, ticker);
 
    //TODO перехватить исключение
    public async Task LeaveTickerSubscription(string ticker) =>
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, ticker);
}