using Foundation;
using MCWeather.iOS.Controllers;
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
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            var listViewController = new WeatherListViewController();
            Window.RootViewController = new UINavigationController(listViewController);
            Window.MakeKeyAndVisible();

            return true;
        }
    }
}

