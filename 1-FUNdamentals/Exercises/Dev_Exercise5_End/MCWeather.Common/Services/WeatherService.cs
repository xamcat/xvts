using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Services.Interfaces;
using Toolbox.Portable.Services;

namespace MCWeather.Common.Services
{
    public class WeatherService
        : BaseHttpService, IWeatherService
    {
        public WeatherService()
            : base("https://xvtsfapktnxnh4rzvvie.azurewebsites.net/api/functions/Weather/")
        { }

        public Task<Weather> GetWeatherAsync(string city, TemperatureUnit unit = TemperatureUnit.Imperial)
        {
            return GetAsync<Weather>($"WeatherByCity?city={city}&unit={unit.ToString().ToLower()}");
        }

        public Task<Weather> GetForecastAsync(int cityId, TemperatureUnit unit = TemperatureUnit.Imperial)
        {
            return GetAsync<Weather>($"ForecastByCityId?cityId={cityId}&units={unit.ToString().ToLower()}");
        }
    }
}
