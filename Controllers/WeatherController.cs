using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/weather/")]

public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private const string SessionKey = "RecentWeatherSearches";

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetWeather(double lat, double lon)
    {

        var history = HttpContext.Session.GetObject<RecentSearchHistory>(SessionKey) ?? new RecentSearchHistory();
        history.Items.Add(new RecentSearch(lat, lon));

        HttpContext.Session.SetObject(SessionKey, history);

        var weather = await  _weatherService.GetCurrentWeatherAsync(lat, lon);
        if (weather == null)
        {
            return BadRequest("Invalid response from API.");
        }

        return Ok(weather);
    }

    [HttpGet("recent")]
    public IActionResult GetRecentSearches()
    {
        var history = HttpContext.Session.GetObject<RecentSearchHistory>(SessionKey)
            ?? new RecentSearchHistory();

        return Ok(history);
    }
}