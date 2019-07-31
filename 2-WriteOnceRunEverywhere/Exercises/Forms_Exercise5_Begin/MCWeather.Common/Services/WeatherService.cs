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
            : base("https://asapinu5ov4uzk6wu6.azurewebsites.net/")
        { }

        public Task<Weather> GetWeatherAsync(string city, TemperatureUnit unit = TemperatureUnit.Imperial)
        {
            return GetAsync<Weather>($"api/forecasts/{city}?units={unit.ToString().ToLower()}", requestMessage =>
            {
                requestMessage.Headers.Add("key", "148a16d1-a235-4caa-9f0d-f44f90737ad5");
            });
        }

        public Task<Weather> GetForecastAsync(int cityId, TemperatureUnit unit = TemperatureUnit.Imperial)
        {
            return GetAsync<Weather>($"ForecastByCityId?cityId={cityId}&units={unit.ToString().ToLower()}");
        }
    }
}
