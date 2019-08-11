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
            : base("https://asapikjy6zobfbf6xe.azurewebsites.net/")
        { }

        public Task<Weather> GetWeatherAsync(string city, TemperatureUnit unit = TemperatureUnit.Imperial)
        {
            return GetAsync<Weather>($"api/forecasts/{city}?units={unit.ToString().ToLower()}", requestMessage =>
            {
                requestMessage.Headers.Add("key", "B50996A0-9D60-44AF-BF08-81029CE2B8C7");
            });
        }
    }
}
