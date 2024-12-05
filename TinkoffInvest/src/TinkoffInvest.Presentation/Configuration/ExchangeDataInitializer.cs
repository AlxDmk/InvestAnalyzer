using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TinkoffInvest.Data;
using TinkoffInvest.Data.Entities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TinkoffInvest.Presentation.Configuration;

public static class ExchangeDataInitializer
{
    public static async Task<WebApplication> UseExchangeDataInitializer(this WebApplication app, CancellationToken token)
    {
        using var scope = app.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TinkoffInvestDbContext>();
        try
        {
            await context.Database.EnsureCreatedAsync(token);

            var securities = context.Securities.FirstOrDefault();
            if (securities is null)
            {
                // Заполнение БД данными об акциях
                var shares =  await File.ReadAllTextAsync($"{Directory.GetParent(Directory.GetCurrentDirectory())}/src/TinkoffInvest/src/TinkoffInvest.Data/shares.json", token);
                List<SecurityEntity>? sharesEntities = JsonSerializer.Deserialize<List<SecurityEntity>>(shares);

                if (sharesEntities != null) await context.AddRangeAsync(sharesEntities, token);
                //Заполение БД данными об облигациях
                var bonds =  await  File.ReadAllTextAsync($"{Directory.GetParent(Directory.GetCurrentDirectory())}/src/TinkoffInvest/src/TinkoffInvest.Data/bonds.json", token);
                List<SecurityEntity>? bondsEntities = JsonSerializer.Deserialize<List<SecurityEntity>>(bonds);
              
                if (bondsEntities != null) await context.AddRangeAsync(bondsEntities, token);
                await context.SaveChangesAsync(token);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return app;
    }
}