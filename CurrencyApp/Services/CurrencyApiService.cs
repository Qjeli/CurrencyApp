using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyApp.Models;
using Newtonsoft.Json;

namespace CurrencyApp.Services
{
    public class CurrencyApiService
    {
        private const string ApiUrl = "https://www.cbr-xml-daily.ru/daily_json.js";
        private static readonly HttpClient _httpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        public async Task<List<Currency>> FetchCurrenciesAsync()
        {
            var json = await _httpClient.GetStringAsync(ApiUrl);
            var resp = JsonConvert.DeserializeObject<CbrResponse>(json);

            return resp?.Valute?.Values
                .Select(x => new Currency
                {
                    CharCode = x.CharCode,
                    NumCode = x.NumCode,
                    Name = x.Name,
                    Value = x.Value,
                    Nominal = x.Nominal,
                    Previous = x.Previous,
                    IsUserAdded = false
                })
                .ToList() ?? new List<Currency>();
        }

    }
}