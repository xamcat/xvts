namespace MCWeather.Common.Models
{
    public class WeatherRequest
    {
        public override string ToString()
        {
            return City?.Name;
        }

        public City City
        {
            get;
            set;
        }

        public string CityName
        {
            get => this.City?.Name;
        }

    }
}