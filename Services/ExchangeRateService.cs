using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using dot.Data;
using dot.Models;
using System.Collections;
using System.Globalization;


namespace dot.Services
{
    public class ExchangeRateService
    {
        private readonly AppDbContext _context;

        public ExchangeRateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateExchangeRates()
        {
            // Retrieve exchange rates from an open source API (for example, ExchangeRatesAPI)
            string apiUrl = "https://open.er-api.com/v6/latest/USD";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var exchangeRates = JsonSerializer.Deserialize<ExchangeRateApiResponseDto>(json, options);

                    // Save the exchange rates to the database using Entity Framework Core
                    var existingRates = _context.ExchangeRates.ToList();
                    var now = DateTime.Now;

                    foreach (KeyValuePair<string, decimal> rate in exchangeRates.Rates)
                    {
                        string currencyCode = rate.Key;
                        decimal rateValue = rate.Value;
                        string format = "ddd, dd MMM yyyy HH:mm:ss zzz";
                        DateTime date = DateTime.ParseExact(exchangeRates.time_last_update_utc, format, CultureInfo.InvariantCulture).ToUniversalTime();
                        var existingRate = existingRates.FirstOrDefault(r => r.Timestamp == date);
                        if (existingRate == null)
                        {
                            _context.ExchangeRates.Add(new ExchangeRate
                            {
                                Currency = currencyCode,
                                Rate = rateValue,
                                Timestamp = date,
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve exchange rates from {apiUrl} with status code {response.StatusCode}.");
                }
            }
        }


        public class ExchangeRateApiResponseDto
        {
            public string time_last_update_utc { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }
        }


        public List<ExchangeRate> GetExchangeRates()
        {
            return _context.ExchangeRates.ToList();
        }
    }
}
