using MCWeather.Common.Models;
using Xamarin.Forms;

namespace MCWeather.Forms.Views
{
    public partial class LocationViewCell : ViewCell
    {
        public LocationViewCell()
        {
            InitializeComponent();
            this.CityLabel.SetBinding(Label.TextProperty, nameof(WeatherRequest.CityName));
            this.CurrentTemperatureLabel.SetBinding(Label.TextProperty, nameof(LocationWeatherRequest.CurrentTemperature));
        }
    }
}