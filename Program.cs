using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        double lat = 50.0219746;
        double lon = -125.2428408;

        string apiKey = config["WeatherApi:ApiKey"]!;
        string baseUrl = config["WeatherApi:BaseUrl"]!;

        string url = $"{baseUrl}?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

        HttpClient client = new HttpClient();
        try
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: API returned {response.StatusCode}");
                return;
            }

            string json = await response.Content.ReadAsStringAsync();

            WeatherResponse weather = JsonSerializer.Deserialize<WeatherResponse>(json);

            Console.WriteLine($"Weather: {weather.weather?[0]?.main} - {weather.weather?[0]?.description}");
            Console.WriteLine($"Temp: {weather.main?.temp}°C, Feels like: {weather.main?.feels_like}°C");
            Console.WriteLine($"Wind: {weather.wind?.speed} m/s, Direction: {weather.wind?.deg}°");

        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong: " + e.Message);
        }

    }
}