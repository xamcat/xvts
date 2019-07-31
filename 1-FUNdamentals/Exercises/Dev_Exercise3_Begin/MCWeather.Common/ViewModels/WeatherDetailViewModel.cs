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

        string _cityName;
        string _backgroundPath;
        int? _lowTemperature;
        int? _highTemperature;
        int? _currentTemperature;
        string _description;

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
            // TODO: Lookup weather information and update properties
        }
    }
}