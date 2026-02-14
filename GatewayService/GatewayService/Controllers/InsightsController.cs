using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GatewayService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsightsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public InsightsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _config = configuration;
        }

        private string DataServiceBaseUrl()
        {
            var baseUrl = _config["DataService:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("Data service base URL is not configured.");
            }

            return baseUrl.TrimEnd('/');
        }

        // GET /api/insights/curent
        //Returns: latest readings + current weather insights(Open-Meteo)
        [HttpGet("current")]
        public async Task<IActionResult> Current()
        {
            //(1) Get latest reading from Data Service
            var readingsUrl = $"{DataServiceBaseUrl()}/readings";
            var readingsJson = await _httpClient.GetStringAsync(readingsUrl);

            using var doc = JsonDocument.Parse(readingsJson);
            var root = doc.RootElement;

            if(root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
            {
                return NotFound("No readings found in DataService.");
            }

            var latestReading = root[0];

            //2) Call external weather API (Open-Meteo)

            var weatherUrl = "https://api.open-meteo.com/v1/forecast?latitude=44.8&longitude=20.5&current=temperature_2m,relative_humidity_2m,wind_speed_10m";
            var wheaterJson = await _httpClient.GetStringAsync(weatherUrl);

            //3) Return integrated response

            return Ok(new
            {
                latestReading = JsonDocument.Parse(latestReading.GetRawText()).RootElement,
                weather = JsonDocument.Parse(wheaterJson).RootElement
            });
        }
    }
}
