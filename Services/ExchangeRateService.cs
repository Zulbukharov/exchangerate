using System.Text.Json;
using dot.Data;
using dot.Models;
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

                    var existingRates = _context.ExchangeRates.ToList();
                    var now = DateTime.Now;
                    string format = "ddd, dd MMM yyyy HH:mm:ss zzz";
                    DateTime date = DateTime.ParseExact(exchangeRates.time_last_update_utc, format, CultureInfo.InvariantCulture).ToUniversalTime();

                    foreach (KeyValuePair<string, decimal> rate in exchangeRates.Rates)
                    {
                        string currencyCode = rate.Key;
                        decimal rateValue = rate.Value;
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


        // public List<ExchangeRate> GetExchangeRates()
        // {
        //     return _context.ExchangeRates.ToList();
        // }

        public List<ExchangeRate> GetExchangeRatesForToday()
        {
            DateTime today = DateTime.UtcNow.Date;
            return _context.ExchangeRates.Where(x => (
                x.Timestamp.Day == today.Day &&
                x.Timestamp.Month == today.Month &&
                x.Timestamp.Year == today.Year
            )).ToList();
        }

    }
}
