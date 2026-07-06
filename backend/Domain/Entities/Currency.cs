using System.Globalization;

namespace backend.Domain.Entities;

public class Currency
{
    public Currency(string symbol, string isoSymbol, string fullName)
    {
        var allCurrencies = CurrencyHelpers.GetAllCurrencies();
        
        var symbolMatches = allCurrencies.Where(r =>
            r.CurrencySymbol.Contains(symbol)).ToList();

        var isoMatches = allCurrencies.Where(r =>
            r.ISOCurrencySymbol.Contains(isoSymbol)).ToList();
        
        var fullNameMatches = allCurrencies.Where(r =>
            r.CurrencyEnglishName.Contains(fullName)).ToList();
        
        
        if (symbolMatches.Count == 0)
        {
            throw new ArgumentException("No currencies with symbol specified found", nameof(symbol));
        }
        else if (isoMatches.Count == 0)
        {
            throw new ArgumentException("No currencies with isoSymbol specified found", nameof(isoSymbol));
        }
        else if (fullNameMatches.Count == 0)
        {
            throw new ArgumentException("No currencies with fullName specified found", nameof(fullName));
        }
        Symbol = symbol;
        IsoSymbol = isoSymbol;
        FullName = fullName;
    }
    
    private Currency() { }
    
    public string Symbol { get; private set; }
    public string IsoSymbol { get; private set; }
    public string FullName { get; private set; }
}

public static class CurrencyHelpers
{
    public static IOrderedEnumerable<RegionInfo> GetAllCurrencies()
    {
        return CultureInfo
            .GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new RegionInfo(c.Name))
            .GroupBy(r => r.ISOCurrencySymbol)
            .Select(g => g.First())
            .OrderBy<RegionInfo, string>(r => r.ISOCurrencySymbol);
    }
}