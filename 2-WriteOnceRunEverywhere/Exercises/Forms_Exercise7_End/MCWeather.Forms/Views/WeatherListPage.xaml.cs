using System;
using MCWeather.Common.ViewModels;
using MCWeather.Common.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using MCWeather.Forms.Converters;

namespace MCWeather.Forms.Views
{
    public partial class WeatherListPage : ContentPage
    {
        private WeatherRequestDataTemplateSelector WeatherRequestDataTemplateSelector { get; set; }
        public WeatherListViewModel ViewModel { get; private set; }

        public Action<WeatherRequest> WeatherSelected;

        public WeatherListPage()
        {
            InitializeComponent();

            WeatherRequestDataTemplateSelector = new WeatherRequestDataTemplateSelector();
            ViewModel = new WeatherListViewModel();
            BindingContext = ViewModel;

            WeatherList.SetBinding(ItemsView<Cell>.ItemsSourceProperty, nameof(ViewModel.WeatherRequests));
            WeatherList.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.RefreshCommand));
            WeatherList.SetBinding(ListView.IsRefreshingProperty, nameof(ViewModel.IsRefreshing), BindingMode.OneWay);
            WeatherList.ItemTemplate = WeatherRequestDataTemplateSelector;
            AddFavoriteToolbarItem.SetBinding(MenuItem.CommandProperty, nameof(ViewModel.AddFavoriteCommand));
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
            var weatherDetailView = new WeatherDetailPage();

            switch (e.SelectedItem)
            {
                case LocationWeatherRequest lwr:
                    weatherDetailView.ViewModel.SetWeather(lwr.City.Name, lwr.Weather);
                    break;
                case WeatherRequest wr:
                    weatherDetailView.ViewModel.SetWeatherRequest(wr);
                    break;
            }

            WeatherSelected?.Invoke(e.SelectedItem as WeatherRequest);
        }
    }
}