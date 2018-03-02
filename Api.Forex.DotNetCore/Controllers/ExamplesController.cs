using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Forex.DotNetCore.Models;
using Api.Forex.Sharp;
using Api.Forex.Sharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Api.Forex.DotNetCore.Controllers
{
    public class ExamplesController : Controller
    {
        private static IMemoryCache _Cache;
        public static IConfiguration _Configuration { get; set; }
        public ExamplesController(IConfiguration Configuration, IMemoryCache memoryCache)
        {
            _Configuration = Configuration;
            _Cache = memoryCache;
        }

        [Route("example/javascript-currency-converter", Order = 100)]
        public async Task<IActionResult> Example1()
        {
            string ApiForexKey = _Configuration["ApiForex:Key"];
            //DailyRates ForexRates = await _Cache.GetOrCreateAsync("FxRatesJson", async entry =>
            //{
            //    ForexRates = await ApiForex.GetRate(ApiForexKey, "json");
            //    //entry.SlidingExpiration = TimeSpan.FromMinutes((DateTime.UtcNow - Convert.ToDateTime(ForexRates.timestamp)).TotalMinutes + 5);
            //    entry.SlidingExpiration = TimeSpan.FromMinutes(60);
            //    return ForexRates;
            //});
            string ForexRatesJson = await _Cache.GetOrCreateAsync("ForexRatesJson", async entry =>
            {
                ForexRatesJson = await ApiForex.GetRateJson(ApiForexKey, "USD");
                //entry.SlidingExpiration = TimeSpan.FromMinutes((DateTime.UtcNow - Convert.ToDateTime(ForexRates.timestamp)).TotalMinutes + 5);
                entry.SlidingExpiration = TimeSpan.FromMinutes(60);
                return ForexRatesJson;
            });
            Dictionary<string, CurrencyInfos> CurrenciesInfos = await _Cache.GetOrCreateAsync("CurrenciesInfos", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(60);
                return await ApiForex.GetCurrenciesInfos("en"); ;
            });
            return View(new Example1ViewModel
            {
                ForexRatesJson = ForexRatesJson,
                CurrenciesInfos = CurrenciesInfos
            });
        }
        [Route("example/prices-converter", Order = 100)]
        public async Task<IActionResult> Example2()
        {
            // Your Api key configurable in appsettings.json
            string ApiForexKey = _Configuration["ApiForex:Key"];
            // List of currencies to convert (optionnal)
            string[] CurrenciesList = new string[] { "EUR", "USD", "JPY", "GBP", "AUD", "CAD", "CNY" };
            // Retreive the currencies and add it to cache for 60 minutes
            // https://v1.api.forex/daily-rates.json?to=EUR,USD,JPY,GBP,AUD,CAD,CNY&key={YourKey}
            ApiForexRates ForexRates = await _Cache.GetOrCreateAsync("ForexRates", async entry =>
            {
                ForexRates = await ApiForex.GetRate(ApiForexKey, "proto", "USD", CurrenciesList);
                entry.SlidingExpiration = TimeSpan.FromMinutes(60);
                return ForexRates;
            });
            // Retreive the currencies infos and add it to cache for 30 days
            // https://v1.api.forex/infos.json?currencies=EUR,USD,JPY,GBP,AUD,CAD,CNY&lang=fr&key={YourKey}
            Dictionary <string, CurrencyInfos> CurrenciesInfos = await _Cache.GetOrCreateAsync("CurrenciesInfos", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(30);
                return await ApiForex.GetCurrenciesInfos("en", CurrenciesList); ;
            });

            IEnumerable< Example2CurrencyList> CurrenciesMerge = from kvp1 in CurrenciesInfos
                          join kvp2 in ForexRates.rates on kvp1.Key equals kvp2.Key
                          select new Example2CurrencyList { Code = kvp1.Key, Name = kvp1.Value.Name, Symbol = kvp1.Value.Symbol, Price = kvp2.Value };

            return View(new Example2ViewModel { CurrenciesList = CurrenciesMerge } );
        }
        [Route("api/get-rates")]
        public async Task<ContentResult> Example1Ajax()
        {
            //string[] ToCurrencies = new string[] { "EUR", "USD", "THB" };
            string ApiForexKey = _Configuration["ApiForex:Key"];
            string ForexRatesJson = await _Cache.GetOrCreateAsync("ForexRatesJson", async entry =>
            {
                ForexRatesJson = await ApiForex.GetRateJson(ApiForexKey, "USD");
                //entry.SlidingExpiration = TimeSpan.FromMinutes((DateTime.UtcNow - Convert.ToDateTime(ForexRates.timestamp)).TotalMinutes + 5);
                entry.SlidingExpiration = TimeSpan.FromMinutes(60);
                return ForexRatesJson;
            });
            return Content(ForexRatesJson, "application/json");
        }
    }
}