using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/weather")]

public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeather(double lat, double lon)
    {
        var weather = await  _weatherService.GetCurrentWeatherAsync(lat, lon);
        if (weather == null)
        {
            return BadRequest("Invalid response from API.");
        }

        return Ok(weather);
    }
}