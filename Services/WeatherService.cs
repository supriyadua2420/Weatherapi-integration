using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public WeatherService(HttpClient httpClient, IConfiguration config)
    {
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(double lat, double lon)
    {
        string apiKey = _config["WeatherApi:ApiKey"];
        string baseUrl = _config["WeatherApi:BaseUrl"];

        string url = $"{baseUrl}?lat={50.0219}&lon={-125.2428}&appid={apiKey}&units=metric";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        string json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<WeatherResponse>(json);
    }

}
