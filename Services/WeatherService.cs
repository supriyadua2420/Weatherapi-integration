using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;


public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;

    public WeatherService(HttpClient httpClient, IConfiguration config, IMemoryCache cache)
    {
        _config = config;
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(double lat, double lon)
    {
        string cacheKey = $"weather-current-{lat}-{lon}";

        if (_cache.TryGetValue(cacheKey, out WeatherResponse cachedWeather))
        {
            Console.WriteLine("Returning from cache");
            return cachedWeather;
        }

        string apiKey = _config["WeatherApi:ApiKey"];
        string baseUrl = _config["WeatherApi:BaseUrl"];

        string url = $"{baseUrl}?lat={50.0219}&lon={-125.2428}&appid={apiKey}&units=metric";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var weather = await response.Content.ReadFromJsonAsync<WeatherResponse>();

        _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10));

        return weather;
    }

}
