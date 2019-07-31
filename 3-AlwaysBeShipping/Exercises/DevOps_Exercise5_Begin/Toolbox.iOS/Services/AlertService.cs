using System;
using System.Threading.Tasks;
using Toolbox.Portable.Services;
using UIKit;

namespace Toolbox.iOS.Services
{
    public class AlertService : IAlertService
    {
        public Task DisplayAsync(string title, string message = null, string cancelButton = null)
        {
            var tcs = new TaskCompletionSource<object>();

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alertController = UIAlertController.Create(title, message ?? string.Empty, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(cancelButton ?? "Cancel", UIAlertActionStyle.Cancel, (obj) => tcs.TrySetResult(null)));

                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.ShowDetailViewController(alertController, vc);
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
			var tcs = new TaskCompletionSource<string>();

			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var alertController = UIAlertController.Create(title, message ?? string.Empty, UIAlertControllerStyle.Alert);
				alertController.AddAction(UIAlertAction.Create(cancelButton ?? "Cancel", UIAlertActionStyle.Cancel, (obj) => tcs.TrySetResult(null)));
				alertController.AddAction(UIAlertAction.Create(actionButton ?? "Ok", UIAlertActionStyle.Default, (obj) =>
				{
					string value = null;
					var alertTextField = alertController.TextFields[0];
					value = alertTextField.Text;

					if (string.IsNullOrEmpty(value))
						value = hint;
					else
						value = value.Trim();

					tcs.TrySetResult(value);
				}));
				alertController.AddTextField((UITextField textField) =>
				{
					if (hint != null) textField.Placeholder = hint;

					textField.AutocorrectionType = UITextAutocorrectionType.No;
					textField.AutocapitalizationType = UITextAutocapitalizationType.None;

					if (validator != null)
					{
						var acceptButton = alertController.Actions[1];
						acceptButton.Enabled = false;

						textField.AddTarget((sender, e) =>
						{
							string value = textField.Text;

							if (string.IsNullOrEmpty(value))
								value = textField.Placeholder;
							else
								value = value.Trim();

							bool valid = validator(value);

							textField.TextColor = valid ? UIColor.Black : UIColor.Red;

							acceptButton.Enabled = valid;

						}, UIControlEvent.EditingChanged);
					}
				});

				var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
				while (vc.PresentedViewController != null)
				{
					vc = vc.PresentedViewController;
				}

				vc.ShowDetailViewController(alertController, vc);
			});

			return tcs.Task;
		}
    }
}