using Api.Forex.Sharp.Models;

namespace Api.Forex.DotNetCore.Models
{
    public class IndexViewModel
    {
        public ApiForexRates ForexRates { get; set; }
        public string ApiError { get; set; }
    }
}