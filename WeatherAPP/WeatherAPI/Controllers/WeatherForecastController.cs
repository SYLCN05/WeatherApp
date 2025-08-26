using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
      
        private readonly HttpClient _httpClient;
        private readonly string API_KEY = Secrets.OpenWeatherApiKey();
        public WeatherForecastController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{Zipcode}")]
        public async Task<IActionResult> PostalcodeToGeocode([FromRoute] string Zipcode,[FromQuery] string CountryCode)
        {
            _httpClient.BaseAddress = new Uri("http://api.openweathermap.org/geo/1.0/");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.GetAsync($"zip?zip={Zipcode},{CountryCode}&appid={API_KEY}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentData([FromQuery] double lat, [FromQuery] double lon)
        {
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
         

            var response = await _httpClient.GetAsync($"weather?lat={lat}&lon={lon}&appid={API_KEY}&units=metric");

            if (response.IsSuccessStatusCode) 
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
