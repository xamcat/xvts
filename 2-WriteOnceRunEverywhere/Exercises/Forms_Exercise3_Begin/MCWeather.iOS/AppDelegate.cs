using Foundation;
using MCWeather.Common;
using MCWeather.Common.Services;
using MCWeather.iOS.Controllers;
using Toolbox.iOS.Services;
using Toolbox.Portable.Services;
using UIKit;

namespace MCWeather.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var p = SQLite.SQLiteException.New(SQLite.SQLite3.Result.Row, "");

            Bootstrap.Begin(
                () => new LocationService(),
                () => new AlertService(),
                () => FileSystem.RegisterService(new FileSystemService())
            );

            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            var listViewController = new WeatherListViewController();
            Window.RootViewController = new UINavigationController(listViewController);
            Window.MakeKeyAndVisible();

            return true;
        }
    }
}

