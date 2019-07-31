using Faucet.iOS.ViewControllers;
using Foundation;
using UIKit;

namespace Faucet.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            Window.RootViewController = new FishTableViewController();
            Window.MakeKeyAndVisible();

            return true;
        }
    }
}

