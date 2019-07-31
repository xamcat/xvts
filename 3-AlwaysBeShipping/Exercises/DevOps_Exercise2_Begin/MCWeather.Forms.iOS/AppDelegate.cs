using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using MCWeather.Common;
using MCWeather.Common.Services;
using Toolbox.iOS.Services;
using Toolbox.Portable.Services;
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
    }
}