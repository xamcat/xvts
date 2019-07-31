using System;
using MCWeather.Common.Models;
using Xamarin.Forms;

namespace MCWeather.Forms.Converters
{
    public class WeatherRequestDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CityWeatherRequestTemplate { get; set; }
        public DataTemplate LocationWeatherRequestTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if ((item as WeatherRequest) == null)
            {
                throw new InvalidOperationException("Items must be derived from WeatherRequest");
            }

            return item.GetType() == typeof(LocationWeatherRequest) ? LocationWeatherRequestTemplate : CityWeatherRequestTemplate;
        }
    }
}