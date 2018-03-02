Open a free account on https://api.forex and get an **api key**

~~~csharp
DailyRates ForexRates = await ApiForex.GetRate("YourApiForexKey");
~~~

Convert the rates to an other currency.
Example: convert 5 Thai bahts in Euros

~~~csharp
decimal Convert = ForexRates.Convert("THB", "EUR", 5)
~~~
