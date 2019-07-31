using System;
using System.Collections.Generic;
using MCWeather.Forms.Views;
using MCWeather.Common.ViewModels;
using MCWeather.Common.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MCWeather.Forms.Views
{
    public partial class WeatherListView : ContentPage
    {
        public WeatherListViewModel ViewModel { get; private set; }

        public WeatherListView()
        {
            InitializeComponent();

            ViewModel = new WeatherListViewModel();
            BindingContext = ViewModel;

            WeatherList.SetBinding(ListView.ItemsSourceProperty, nameof(ViewModel.WeatherRequests));
            WeatherList.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.RefreshCommand));
            WeatherList.SetBinding(ListView.IsRefreshingProperty, nameof(ViewModel.IsRefreshing), BindingMode.OneWay);
            AddFavoriteToolbarItem.SetBinding(ToolbarItem.CommandProperty, nameof(ViewModel.AddFavoriteCommand));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            WeatherList.ItemSelected += ItemSelected;

            Task.Run(async () =>
            {
                try
                {
                    await ViewModel.InitAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }

        protected override void OnDisappearing()
        {
            WeatherList.ItemSelected -= ItemSelected;
            base.OnDisappearing();
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var weatherDetailView = new WeatherDetailView();

            switch (e.SelectedItem)
            {
                case LocationWeatherRequest lwr:
                    weatherDetailView.ViewModel.SetWeather(lwr.City.Name, lwr.Weather);
                    break;
                case WeatherRequest wr:
                    weatherDetailView.ViewModel.SetWeatherRequest(wr);
                    break;
            }

            Navigation.PushAsync(weatherDetailView);
        }
    }
}