namespace TinkoffInvest.Core.Models;

public class Security
{
    public string Uid { get; set; } = string.Empty;
    public string Figi { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string ClassCode { get; set; } = string.Empty;
    public string Isin { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string InstrumentType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CountryOfRisk { get; set; } = string.Empty;
    public bool LiquidityFlag { get; set; } = false;
    public bool isFavorite { get; set; } = false;
    public bool isLastPriceSubscribed { get; set; } = false;
    public decimal Price { get; set; }
}