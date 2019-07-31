using System;
using System.Threading.Tasks;
using Android.App;
using Toolbox.Droid.Views;
using Toolbox.Portable.Services;

namespace Toolbox.Droid.Services
{
    public class AlertService : IAlertService
    {
        private Activity activity;

        public AlertService(Activity activity)
        {
            this.activity = activity;
        }

        public Task DisplayAsync(string title, string message = null, string cancelButton = null)
        {
            var tcs = new TaskCompletionSource<object>();

            var alertBuilder = new AlertDialog.Builder(activity);
            alertBuilder.SetTitle(title);

            if (message != null)
            {
                alertBuilder.SetMessage(message);
            }

            alertBuilder.SetCancelable(false);

            alertBuilder.SetPositiveButton(cancelButton ?? "Cancel", delegate
            {
                tcs.TrySetResult(null);
            });

            activity.RunOnUiThread(() =>
            {
                alertBuilder.Show();
            });

            return tcs.Task;
        }

        public Task<string> DisplayInputEntryAsync(
            string title,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            string hint = null,
            Func<string, bool> validator = null) 

        {
            var textInputDialog = new InputDialogFragment(this.activity, title, message, actionButton, cancelButton, hint, validator);

            this.activity.RunOnUiThread(() =>
            {
                textInputDialog.Show(this.activity.FragmentManager, "dialog");
            });

            return textInputDialog.Tcs.Task;
        }
    }
}