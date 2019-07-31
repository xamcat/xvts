using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace Toolbox.Droid.Views
{
	public class InputDialogFragment : DialogFragment, ITextWatcher, IDialogInterfaceOnShowListener, View.IOnClickListener
	{
		private WeakReference<Context> contextWeak;

		private readonly string title, message, actionButton, cancelButton, hint;
		private readonly Func<string, bool> validator;

		private IDialogInterface dialog;
		private EditText textInput;

		public TaskCompletionSource<string> Tcs { get; }

		public InputDialogFragment(
			Context context,
			string title,
			string message = null,
			string actionButton = null,
			string cancelButton = null,
			string hint = null,
			Func<string, bool> validator = null)
		{
			this.contextWeak = new WeakReference<Context>(context);
			this.title = title;
			this.message = message;
			this.actionButton = actionButton;
			this.cancelButton = cancelButton;
			this.hint = hint;
			this.validator = validator;

			this.Tcs = new TaskCompletionSource<string>();
		}

		public override Dialog OnCreateDialog(Android.OS.Bundle savedInstanceState)
		{
			contextWeak.TryGetTarget(out Context context);

			var alertBuilder = new Android.Support.V7.App.AlertDialog.Builder(context);
			alertBuilder.SetTitle(title);

			if (message != null)
				alertBuilder.SetMessage(message);

			textInput = new EditText(context);
			textInput.AddTextChangedListener(this);
			alertBuilder.SetView(textInput);

			if (hint != null)
			{
				textInput.Text = hint;
				textInput.SelectAll();
			}

			alertBuilder.SetCancelable(false);

			alertBuilder.SetNegativeButton(cancelButton ?? "Cancel", delegate
			{
				Tcs.TrySetResult(null);
			});

			alertBuilder.SetPositiveButton(actionButton ?? "Ok", (EventHandler<DialogClickEventArgs>)null);

			var alert = alertBuilder.Create();

			alert.SetOnShowListener(this);

			return alert;
		}

		public void AfterTextChanged(IEditable s)
		{
			if (validator == null) return;

			if (validator(s.ToString()))
				textInput.SetTextColor(Android.Graphics.Color.Black);
			else
				textInput.SetTextColor(Android.Graphics.Color.Red);
		}

		public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
		{
		}

		public void OnTextChanged(ICharSequence s, int start, int before, int count)
		{
		}

		public void OnShow(IDialogInterface dialog)
		{
			this.dialog = dialog;
			var button = ((Android.Support.V7.App.AlertDialog)dialog).GetButton((int)DialogButtonType.Positive);
			button.SetOnClickListener(this);
		}


		public void OnClick(View v)
		{
			string value = textInput?.Text;
			if (!string.IsNullOrEmpty(value))
				value = value.Trim();

			if (validator != null)
			{
				if (validator(value))
				{
					Tcs.TrySetResult(value);
					dialog.Dismiss();
					return;
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Not Valid");
					return;
				}
			}
			else
			{
				Tcs.TrySetResult(value);
				dialog.Dismiss();

				return;
			}
		}
	}
}