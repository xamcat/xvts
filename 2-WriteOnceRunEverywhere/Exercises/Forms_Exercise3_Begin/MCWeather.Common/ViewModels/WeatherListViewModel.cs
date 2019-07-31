using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Services.Interfaces;
using Toolbox.MVVM.Input;
using Toolbox.Portable.Infrastructure;
using Toolbox.Portable.Mvvm;
using Toolbox.Portable.Services;

namespace MCWeather.Common.ViewModels
{
    public class WeatherListViewModel : BaseViewModel
    {
        IFavoritesService _favoritesService = ServiceContainer.Resolve<IFavoritesService>();
        ILocationService _locationService = ServiceContainer.Resolve<ILocationService>();
        IAlertService _alertService = ServiceContainer.Resolve<IAlertService>();
        IWeatherService _weatherService = ServiceContainer.Resolve<IWeatherService>();

        ObservableCollection<WeatherRequest> _weatherRequests = new ObservableCollection<WeatherRequest>();
        bool _isRefreshing = false;

        public Action ReloadAction { get; set; }

        public Command RefreshCommand => new Command(async () => await this.OnRefresh());

        public Command AddFavoriteCommand => new Command(async () => await this.AddFavorite());

        public ObservableCollection<WeatherRequest> WeatherRequests
        {
            get { return _weatherRequests; }
            set { RaiseAndUpdate(ref _weatherRequests, value); }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { RaiseAndUpdate(ref _isRefreshing, value); }
        }

        public override Task InitAsync()
        {
            return LoadDataAsync();
        }

        private async Task OnRefresh()
        {
            if (IsRefreshing)
                return;

            try
            {
                IsRefreshing = true;
                await LoadDataAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task LoadDataAsync()
        {
            var currentLocationTask = _locationService.GetCurrentLocationAsync();
            var favoriteCitiesTask = _favoritesService.GetFavoriteCitiesAsync();

            await Task.WhenAll(currentLocationTask, favoriteCitiesTask).ConfigureAwait(false);

            var currentLocation = currentLocationTask.Result;
            var favoriteCities = favoriteCitiesTask.Result;

            var requests = new ObservableCollection<WeatherRequest>(favoriteCities.Select(c => new WeatherRequest
            {
                City = c
            }));

            if (currentLocation != null)
            {
                var lwr = new LocationWeatherRequest
                {
                    City = new City { Name = "Current Location" },
                    Position = new Position { Latitude = currentLocation.Latitude, Longitude = currentLocation.Longitude }
                };

                requests.Insert(0, lwr);
            }

            WeatherRequests = requests;

            ReloadAction?.Invoke();
        }

        private async Task AddFavorite()
        {
            var favoriteText = await _alertService.DisplayInputEntryAsync("MCWeather", "Enter city name", validator: (text) =>
            {
                return WeatherRequests?.Where((WeatherRequest i) => (i as WeatherRequest)?.City?.Name == text).Count() == 0;
            });

            if (!string.IsNullOrWhiteSpace(favoriteText))
            {
                Weather weather = null;

                try
                {
                    weather = await _weatherService.GetWeatherAsync(favoriteText);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    if (weather == null)
                    {
                        await _alertService.DisplayAsync("MCWeather", $"Cannot find weather data for {favoriteText}", "OK");
                    }
                    else
                    {
                        var favoriteCity = await _favoritesService.AddFavoriteCityAsync(new City { Name = favoriteText });
                        WeatherRequests.Add(new WeatherRequest { City = new City { Name = favoriteText } });
                        ReloadAction?.Invoke();
                    }
                }
            }
        }
    }
}
