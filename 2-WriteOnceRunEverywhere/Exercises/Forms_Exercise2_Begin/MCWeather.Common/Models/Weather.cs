using System;
namespace MCWeather.Common.Models
{
    public class Weather
    {
        public string Name { get; set; }

        public double CurrentTemperature { get; set; }

        public double MinTemperature { get; set; }

        public double MaxTemperature { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public string Overview { get; set; }
    }
}
