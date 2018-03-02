### Api Forex .Net Core ###

The sample demonstrates how to retrieve the Api.Forex currencies rates and how to use it on a C# .Net Core application.

Open a free account on https://api.forex and get an **api key**

~~~csharp
DailyRates ForexRates = await ApiForex.GetRates("YourApiForexKey");
~~~

We can cache the rates (optionnal): 

~~~csharp
ApiForexRates ForexRates = await _Cache.GetOrCreateAsync("FxRatesHome", async entry =>
  {
    ForexRates = await ApiForex.GetRate("YourApiForexKey");
    entry.SlidingExpiration = TimeSpan.FromMinutes(60);
    return ForexRates;
});
~~~

Convert the rates to an other currency.
Example: convert 5 Thai bahts in Euros

~~~csharp
decimal Convert = ForexRates.Convert("THB", "EUR", 5)
~~~
