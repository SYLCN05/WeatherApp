using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using WeatherAPI.Services;
using System.Text.Json;

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
          
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.GetAsync($"http://api.openweathermap.org/geo/1.0/zip?zip={Zipcode},{CountryCode}&appid={API_KEY}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(result);

                var lat = doc.RootElement.GetProperty("lat").GetDouble();
                var lon = doc.RootElement.GetProperty("lon").GetDouble();
                var current = await GetCurrentData(lat, lon);

                return Ok(current);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentData([FromQuery] double lat, [FromQuery] double lon)
        {

            var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API_KEY}&units=metric");

            if (response.IsSuccessStatusCode) 
            {
                var result = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(result);

                var formatted = new
                {
                    Stad = doc.RootElement.GetProperty("name").GetString(),
                    Temp = doc.RootElement.GetProperty("main").GetProperty("temp").GetDouble(),
                    mintemp = doc.RootElement.GetProperty("main").GetProperty("temp_min").GetDouble()
                };

                return Ok(formatted);

            }
            return BadRequest();
        }
    }
}
