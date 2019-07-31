using System;
using System.Collections.Generic;
using MCWeather.Common.ViewModels;
using Xamarin.Forms;
using MCWeather.Forms.Converters;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MCWeather.Forms.Views
{
    public partial class WeatherDetailPage : ContentPage
    {
        public WeatherDetailViewModel ViewModel { get; private set; }

        public WeatherDetailPage()
        {
            InitializeComponent();

            ViewModel = new WeatherDetailViewModel();
            BindingContext = ViewModel;

            DescriptionLabel.SetBinding(Label.TextProperty, nameof(ViewModel.Description));
            CurrentTemperatureLabel.SetBinding(Label.FormattedTextProperty, nameof(ViewModel.CurrentTemperatureFormatted), converter: new TemperatureFormatterConverter());
            HighAndLowTemperatureLabel.SetBinding(Label.TextProperty, nameof(ViewModel.HighAndLowTemperatureFormatted));
            WeatherImage.SetBinding(Image.SourceProperty, nameof(ViewModel.BackgroundPath));
            SetBinding(Page.TitleProperty, new Binding(nameof(ViewModel.CityName)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

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
    }
}