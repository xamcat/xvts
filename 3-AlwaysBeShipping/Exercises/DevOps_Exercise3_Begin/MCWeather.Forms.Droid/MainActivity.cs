using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MCWeather.Common;
using MCWeather.Common.Services;
using Toolbox.Portable.Services;
using Toolbox.Droid.Services;
using Toolbox.Portable.Infrastructure;
using MCWeather.Common.Services.Interfaces;
using MCWeather.Common.Models;

namespace MCWeather.Forms.Droid
{
    [Activity(Label = "MCWeather.Forms.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Bootstrap.Begin(() => new LocationService(this), () => new AlertService(this), () =>
            {
                FileSystem.RegisterService(new FileSystemService());
            });

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        [Java.Interop.Export("addFavorite")]
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
