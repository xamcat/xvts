using System;
namespace MCWeather.Common.Models
{
    public class CityBackground : BaseDataObject
    {
        public string ImageUrl
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string WeatherOverview
        {
            get;
            set;
        }
    }
}
