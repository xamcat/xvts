using System;
using Foundation;

namespace Toolbox.iOS
{
    public static class Platform
    {
        static string applicationSupportPath;
        public static string ApplicationSupportPath
        {
            get {
                if (string.IsNullOrEmpty(applicationSupportPath))
                {
                    var urls = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User);
                    applicationSupportPath = urls[0].Path;
                }
                return applicationSupportPath;
            }
        }
    }
}
