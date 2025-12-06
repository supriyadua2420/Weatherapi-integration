using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IWeatherService
{
    Task<WeatherResponse?> GetCurrentWeatherAsync(double lat, double lon);
}
