using System;
using UIKit;
using Toolbox.Portable.Infrastructure;
using MCWeather.Common.Services.Interfaces;
using System.Threading.Tasks;
using Toolbox.Portable.Services;

namespace MCWeather.iOS.Controllers
{
    public class WeatherListViewController : UIViewController
    {
        public override void LoadView()
        {
            base.LoadView();

            View.BackgroundColor = UIColor.White;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

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
