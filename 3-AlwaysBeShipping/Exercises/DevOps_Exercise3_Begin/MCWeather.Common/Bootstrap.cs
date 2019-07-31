using System;
using Toolbox.Portable.Infrastructure;
using MCWeather.Common.Services;
using SQLite;
using Toolbox.Portable.Services;
using System.IO;
using MCWeather.Common.Models;
using MCWeather.Common.Services.Interfaces;
using MCWeather.Common.Repositories.Interfaces;
using MCWeather.Common.Repositories.SQLite;
using MCWeather.Common.Repositories.Memory;

namespace MCWeather.Common
{
    public static class Bootstrap
    {
        public static void Begin(
            Func<ILocationService> locationService,
            Func<IAlertService> alertService,
            Action platformSpecificBegin,
            bool persistData = false)
        {
            ServiceContainer.Register<ILocationService>(locationService);
            ServiceContainer.Register<IAlertService>(alertService);
            ServiceContainer.Register<IImageService>(() => new ImageService());
            ServiceContainer.Register<IFavoritesService>(() => new FavoritesService());
            ServiceContainer.Register<IWeatherService>(() => new WeatherService());
            platformSpecificBegin();

            if (persistData)
                ServiceContainer.Register<IRepositoryManager>(() => new SQLiteRepositoryManager());
            else
                ServiceContainer.Register<IRepositoryManager>(() => new MemoryRepositoryManager());
        }
    }
}