using Serilog;
using TinkoffInvest.Presentation;
using TinkoffInvest.Presentation.Configuration;
using TinkoffInvest.Presentation.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(TinkoffInvestPresentationEntryPoint).Assembly);

builder.Services.InstallServices(builder.Configuration, typeof(TinkoffInvestPresentationEntryPoint).Assembly);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
#region DI TINKOFF INVEST

builder.Services.AddTinkoffInvestGrpcApiClient((_, settings) =>
{
    settings.Token = builder.Configuration.GetValue<string>("TinkoffInvest:Token");
    settings.TrainingMode = builder.Configuration.GetValue<bool>("TinkoffInvest:TrainingMode");
});

#endregion

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors("React");
app.UseAuthorization();

app.MapControllers();
await app.UseExchangeDataInitializer(CancellationToken.None);
app.MapHub<StockFeedHub>("stocks-feed");

app.Run();