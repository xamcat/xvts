using System;
namespace MCWeather.Common.Models
{
    public class Weather
    {
        public string Name { get; set; }

        public int CurrentTemperature { get; set; }

        public int MinTemperature { get; set; }

        public int MaxTemperature { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public string Overview { get; set; }
    }
}
