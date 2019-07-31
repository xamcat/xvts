using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using MCWeather.Common;
using Toolbox.Droid.Services;
using Toolbox.Portable.Services;

namespace MCWeather.Droid
{
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        private static bool isInitialized = false;

        public MainApplication(IntPtr handle, JniHandleOwnership transfer)
          : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            this.RegisterActivityLifecycleCallbacks(this);
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            this.UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            if (isInitialized) return;
            isInitialized = true;
            var p = SQLite.SQLiteException.New(SQLite.SQLite3.Result.Row, "");

            Bootstrap.Begin(
                () => new LocationService(activity),
                () => new AlertService(activity),
                () => FileSystem.RegisterService(new FileSystemService())
            );
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}