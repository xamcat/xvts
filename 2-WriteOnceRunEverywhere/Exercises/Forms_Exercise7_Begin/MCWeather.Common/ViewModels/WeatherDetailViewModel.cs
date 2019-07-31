using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Services.Interfaces;
using Toolbox.Portable.Infrastructure;
using Toolbox.Portable.Mvvm;
using Toolbox.Portable.Services;
using System.Linq;
using System.Diagnostics;

namespace MCWeather.Common.ViewModels
{
    public class WeatherDetailViewModel : BaseViewModel
    {
        IAlertService _alertService = ServiceContainer.Resolve<IAlertService>();
        ILocationService _locationService = ServiceContainer.Resolve<ILocationService>();
        IWeatherService _weatherService = ServiceContainer.Resolve<IWeatherService>();
        IImageService _imageService = ServiceContainer.Resolve<IImageService>();

        string _cityName;
        string _backgroundPath;
        int? _lowTemperature;
        int? _highTemperature;
        int? _currentTemperature;
        string _description;
        object _needsUpdateFlag = new object();

        public string CityName
        {
            get { return _cityName; }
            set { RaiseAndUpdate(ref _cityName, value); }
        }

        public string BackgroundPath
        {
            get { return _backgroundPath; }
            set { RaiseAndUpdate(ref _backgroundPath, value); }
        }

        public string Description
        {
            get { return _description; }
            set { RaiseAndUpdate(ref _description, value); }
        }

        public int? CurrentTemperature
        {
            get { return _currentTemperature; }
            set
            {
                if (RaiseAndUpdate(ref _currentTemperature, value))
                    Raise(nameof(CurrentTemperatureFormatted));
            }
        }

        public int? HighTemperature
        {
            get { return _highTemperature; }
            set
            {
                if (RaiseAndUpdate(ref _highTemperature, value))
                    Raise(nameof(HighAndLowTemperatureFormatted));
            }
        }

        public int? LowTemperature
        {
            get { return _lowTemperature; }
            set
            {
                if (RaiseAndUpdate(ref _lowTemperature, value))
                    Raise(nameof(HighAndLowTemperatureFormatted));
            }
        }

        private string TemperatureUnitString => this.CurrentTemperature == null ? string.Empty : "°F";

        public string HighAndLowTemperatureFormatted => (HighTemperature != null && LowTemperature != null) ? $"High: {HighTemperature} {TemperatureUnitString} Low: {LowTemperature} {TemperatureUnitString}" : string.Empty;

        public string CurrentTemperatureFormatted => $"{CurrentTemperature} {TemperatureUnitString}";

        public void SetWeatherRequest(WeatherRequest weatherRequest)
        {
            Task.Run(async () =>
            {
                try
                {
                    await GetWeatherAsync(weatherRequest);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }

        public void SetWeather(string cityName, Weather weather)
        {
            Task.Run(async () =>
            {
                try
                {
                    if (weather == null)
                        return;

                    this.CityName = cityName;
                    this.Description = weather.Description;
                    this.CurrentTemperature = Convert.ToInt32(weather.CurrentTemperature);
                    this.HighTemperature = Convert.ToInt32(weather.MaxTemperature);
                    this.LowTemperature = Convert.ToInt32(weather.MinTemperature);

                    this.BackgroundPath = await _imageService.GetBackgroundImageAsync(this.CityName, weather.Overview);
                }
                catch (WebException ex)
                {
                    await this._alertService.DisplayAsync("MCWeather", ex.Message, "Done");
                }
            });
        }

        private async Task GetWeatherAsync(WeatherRequest weatherRequest)
        {
            try
            {
                if (weatherRequest == null)
                    return;

                string city = weatherRequest.City.Name;

                if (string.IsNullOrWhiteSpace(city))
                {
                    throw new NullReferenceException("City name cannot be null or whitespace");
                }

                var weather = await _weatherService.GetWeatherAsync(city);

                SetWeather(city, weather);
            }
            catch (WebException ex)
            {
                await this._alertService.DisplayAsync("MCWeather", ex.Message, "Done");
            }
        }
    }
}