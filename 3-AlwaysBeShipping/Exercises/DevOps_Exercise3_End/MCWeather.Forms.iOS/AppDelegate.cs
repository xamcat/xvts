using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using MCWeather.Common;
using MCWeather.Common.Models;
using MCWeather.Common.Services;
using MCWeather.Common.Services.Interfaces;
using Toolbox.iOS.Services;
using Toolbox.Portable.Services;
using Toolbox.Portable.Infrastructure;
using UIKit;

namespace MCWeather.Forms.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            Bootstrap.Begin(() => new LocationService(), () => new AlertService(), () =>
            {
                FileSystem.RegisterService(new FileSystemService());
            });

#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        [Export("addFavorite:")]
        public bool AddFavorite(string favoriteText)
        {
            if (!string.IsNullOrWhiteSpace(favoriteText))
            {
                var favoritesService = ServiceContainer.Resolve<IFavoritesService>();
                var favoriteCity = favoritesService.AddFavoriteCityAsync(new City { Name = favoriteText }).Result;

                return true;
            }

            return false;
        }
    }
}