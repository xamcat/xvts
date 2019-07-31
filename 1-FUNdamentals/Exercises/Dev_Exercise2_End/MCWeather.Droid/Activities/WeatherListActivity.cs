
using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MCWeather.Common.Models;
using System.Threading.Tasks;
using Toolbox.Portable.Infrastructure;
using MCWeather.Common.Services.Interfaces;
using Toolbox.Portable.Services;
using Android.Support.V7.App;

namespace MCWeather.Droid.Activities
{
    [Activity(Label = "MCWeather", MainLauncher = true, Theme = "@style/MyTheme", Icon = "@mipmap/icon")]
    public class WeatherListActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();

            var weatherService = ServiceContainer.Resolve<IWeatherService>();
            var imageService = ServiceContainer.Resolve<IImageService>();

            var alertService = ServiceContainer.Resolve<IAlertService>();

            Task.Run(async () =>
            {
                try
                {
                    var weather = await weatherService.GetWeatherAsync("Bellevue").ConfigureAwait(false);
                    var image = await imageService.GetBackgroundImageAsync("Bellevue", weather.Description).ConfigureAwait(false);

                    await alertService.DisplayAsync("Weather", $"Bellevue: {weather.CurrentTemperature}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }
    }
}

