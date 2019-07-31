using System;
using MCWeather.Common.Models;
using MCWeather.Forms.Views;
using Xamarin.Forms;

namespace MCWeather.Forms.Converters
{
    public class WeatherRequestDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CityWeatherRequestTemplate { get; set; }
        public DataTemplate LocationWeatherRequestTemplate { get; set; }

        public WeatherRequestDataTemplateSelector()
        {
            CityWeatherRequestTemplate = new DataTemplate(() => 
            {
                var textViewCell = new TextCell();
                textViewCell.SetBinding(TextCell.TextProperty, nameof(WeatherRequest.CityName));

                return textViewCell;
            });

            LocationWeatherRequestTemplate = new DataTemplate(() => 
            { 
                return new LocationViewCell();
            });
        }

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