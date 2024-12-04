using Grpc.Core;
using Tinkoff.InvestApi.V1;

namespace TinkoffInvest.Core.TinkoffApiClient;

public class TinkoffInvestGrpcApiClient(CallInvoker callInvoker)
{
    public InstrumentsService.InstrumentsServiceClient Instruments { get;} =
        new InstrumentsService.InstrumentsServiceClient(callInvoker);
    
    public MarketDataService.MarketDataServiceClient MarketData { get; } =
        new MarketDataService.MarketDataServiceClient (callInvoker);
    
    public MarketDataStreamService.MarketDataStreamServiceClient MarketDataStream { get; } =
        new MarketDataStreamService.MarketDataStreamServiceClient(callInvoker);
    
    public OperationsService.OperationsServiceClient Operations { get; } =
        new OperationsService.OperationsServiceClient(callInvoker);
    
    public OperationsStreamService.OperationsStreamServiceClient OperationsStream { get; } =
        new OperationsStreamService.OperationsStreamServiceClient(callInvoker);
    
    public OrdersService.OrdersServiceClient Orders { get; } = new OrdersService.OrdersServiceClient(callInvoker);
    
    public OrdersStreamService.OrdersStreamServiceClient OrdersStream { get; set; } =
        new OrdersStreamService.OrdersStreamServiceClient(callInvoker);
    
    public SandboxService.SandboxServiceClient Sandbox { get; } = new SandboxService.SandboxServiceClient(callInvoker);
    
    public StopOrdersService.StopOrdersServiceClient StopOrders { get; } =
        new StopOrdersService.StopOrdersServiceClient(callInvoker);
    
    public UsersService.UsersServiceClient Users { get; } = new UsersService.UsersServiceClient(callInvoker);
}