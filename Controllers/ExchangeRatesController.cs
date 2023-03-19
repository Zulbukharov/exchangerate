using dot.Services;
using Microsoft.AspNetCore.Mvc;

namespace dot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ExchangeRateService _exchangeRateService;

        public ExchangeRatesController(ExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public IActionResult GetExchangeRates()
        {
            var exchangeRates = _exchangeRateService.GetExchangeRatesForToday();
            if (exchangeRates == null || exchangeRates.Count == 0)
            {
                return NotFound();
            }
            return Ok(exchangeRates);
        }
    }
}
