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
    }
}
