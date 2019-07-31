using System;
namespace Toolbox.Droid
{
    public static class Platform
    {
        static string applicationSupportPath;
        public static string ApplicationSupportPath
        {
            get
            {
                if (string.IsNullOrEmpty(applicationSupportPath))
                {
                    applicationSupportPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                }
                return applicationSupportPath;
            }
        }
    }
}
