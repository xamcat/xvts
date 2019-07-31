using System;
using System.Threading.Tasks;
using MCWeather.Common.Models;

namespace MCWeather.Common.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<Weather> GetWeatherAsync(string city, TemperatureUnit unit = TemperatureUnit.Imperial);

        //Task<Forecast> GetForecastAsync(int id, TemperatureUnit unit = TemperatureUnit.Imperial);
    }
}
