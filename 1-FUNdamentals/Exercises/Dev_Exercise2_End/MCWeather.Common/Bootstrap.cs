using System;
using Toolbox.Portable.Infrastructure;
using MCWeather.Common.Services;
using SQLite;
using Toolbox.Portable.Services;
using System.IO;
using MCWeather.Common.Models;
using MCWeather.Common.Services.Interfaces;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace MCWeather.Common
{
    public static class Bootstrap
    {
        public static void Begin(
            Func<ILocationService> locationService,
            Func<IAlertService> alertService,
            Action platformSpecificBegin)
        {
            ServiceContainer.Register<ILocationService>(locationService);
            ServiceContainer.Register<IAlertService>(alertService);
            ServiceContainer.Register<IImageService>(() => new ImageService());
            ServiceContainer.Register<IWeatherService>(() => new WeatherService());
            platformSpecificBegin();

            AppCenter.Start("android=f5337f89-d999-42fe-8f02-9b54bd5caa6e;ios=8e609003-f838-416e-9756-08c6eda14b94", typeof(Analytics), typeof(Crashes));
        }
    }
}