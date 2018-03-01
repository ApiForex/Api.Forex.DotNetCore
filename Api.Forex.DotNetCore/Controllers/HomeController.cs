using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Forex.DotNetCore.Models;
using Microsoft.Extensions.Configuration;
using Api.Forex.Currency.Converter;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Forex.DotNetCore.Controllers
{
    public class HomeController : Controller
    {
        private static IMemoryCache _Cache;
        public static IConfiguration _Configuration { get; set; }
        public HomeController(IConfiguration Configuration, IMemoryCache memoryCache)
        {
            _Configuration = Configuration;
            _Cache = memoryCache;
        }
        public async Task<IActionResult> Index()
        {
            string ApiForexKey = _Configuration["ApiForex:Key"];
            DailyRates ForexRates = await _Cache.GetOrCreateAsync("FxRates", async entry =>
            {
                ForexRates = await ApiForex.GetRate(ApiForexKey);
                //entry.SlidingExpiration = TimeSpan.FromMinutes((DateTime.UtcNow - Convert.ToDateTime(ForexRates.timestamp)).TotalMinutes + 5);
                entry.SlidingExpiration = TimeSpan.FromMinutes(60);
                return ForexRates;
            });
            return View(new IndexViewModel
            {
                ForexRates = ForexRates,
            });
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
