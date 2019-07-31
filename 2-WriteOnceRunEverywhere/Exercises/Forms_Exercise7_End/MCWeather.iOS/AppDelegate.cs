using Foundation;
using MCWeather.Common;
using MCWeather.Common.Services;
using MCWeather.iOS.Controllers;
using Toolbox.iOS.Services;
using Toolbox.Portable.Services;
using UIKit;
using MCWeather.Forms.Views;
using Xamarin.Forms.Platform.iOS;

namespace MCWeather.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        private UINavigationController _navigation;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var p = SQLite.SQLiteException.New(SQLite.SQLite3.Result.Row, "");

            global::Xamarin.Forms.Forms.Init();

            Bootstrap.Begin(
                () => new LocationService(),
                () => new AlertService(),
                () => FileSystem.RegisterService(new FileSystemService()),
                false
            );

            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var listPage = new WeatherListPage();
            listPage.WeatherSelected += OnWeatherSelected;

            var listViewController = listPage.CreateViewController();
            listViewController.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(
                    "Add",
                    UIBarButtonItemStyle.Plain,
                    (sender, args) =>
                    {
                        listPage.ViewModel.AddFavoriteCommand.Execute(null);
                    }),
                true);

            _navigation = new UINavigationController(listViewController);

            Window.RootViewController = _navigation;
            Window.MakeKeyAndVisible();

            return true;
        }

        void OnWeatherSelected(Common.Models.WeatherRequest obj)
        {
            var detailViewController = new WeatherDetailViewController();
            detailViewController.ViewModel.SetWeatherRequest(obj);

            _navigation.PushViewController(detailViewController, true);
        }
    }
}

