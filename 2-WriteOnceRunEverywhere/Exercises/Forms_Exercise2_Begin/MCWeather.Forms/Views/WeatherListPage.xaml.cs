using System;
using Xamarin.Forms;

namespace MCWeather.Forms.Views
{
    public partial class WeatherListPage : ContentPage
    {
        public WeatherListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            WeatherList.ItemSelected += ItemSelected;
        }

        protected override void OnDisappearing()
        {
            WeatherList.ItemSelected -= ItemSelected;
            base.OnDisappearing();
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var weatherDetailView = new WeatherDetailPage();

            Navigation.PushAsync(weatherDetailView);
        }
    }
}