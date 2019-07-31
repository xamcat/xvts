using System;
namespace MCWeather.Common.Models
{
    public class LocationWeatherRequest : WeatherRequest
    {
        public Position Position
        {
            get;
            set;
        }

        public Weather Weather
        {
            get;
            set;
        }

        public string CurrentTemperature => Weather?.CurrentTemperature == null ? string.Empty : $"{Weather.CurrentTemperature}°F";
    }
}