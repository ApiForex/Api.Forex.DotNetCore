using Api.Forex.Sharp.Models;
using System.Collections.Generic;

namespace Api.Forex.DotNetCore.Models
{
    public class Example1ViewModel
    {
        public string ForexRatesJson { get; set; }
        public ApiForexRates ForexRates { get; set; }
        public Dictionary<string, CurrencyInfos> CurrenciesInfos { get; set; }
    }
    public class Example2ViewModel
    {
        public IEnumerable<Example2CurrencyList> CurrenciesList { get; set; }
    }
    public class Example2CurrencyList
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}