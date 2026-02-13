using GatewayService.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadingController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ReadingController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _config = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateReadingDto dto)
        {
            var baseUrl = _config["DataService:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl)){
                return StatusCode(StatusCodes.Status500InternalServerError, "Data service base URL is not configured.");
            }

            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/readings", dto);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorBody);
            }

            var result = await response.Content.ReadFromJsonAsync<object>();
            return StatusCode(201, result);
        }
    }
}
