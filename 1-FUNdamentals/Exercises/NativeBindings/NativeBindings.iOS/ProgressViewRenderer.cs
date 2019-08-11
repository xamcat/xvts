using System;
using System.ComponentModel;
using NativeBindings;
using NativeBindings.iOS;
using SvProgressHudBinding.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ProgressView), typeof(ProgressViewRenderer))]
namespace NativeBindings.iOS
{
    public class ProgressViewRenderer : ViewRenderer<ProgressView, SVProgressHUD>
    {
        SVProgressHUD _progressHud;

        protected override void OnElementChanged(ElementChangedEventArgs<ProgressView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _progressHud = new SVProgressHUD();
                    //Can set values here
                    SVProgressHUD.SetBackgroundColor(Element.BackgroundColor.ToUIColor());
                    if (Element.IsDisplayed)
                    {
                        SVProgressHUD.Show();
                    }
                    else
                    {
                        SVProgressHUD.Dismiss();
                    }
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(ProgressView.IsDisplayed))
            {
                if (Element.IsDisplayed)
                {
                    SVProgressHUD.Show();
                }
                else
                {
                    SVProgressHUD.Dismiss();
                }
            }
            else if (e.PropertyName == nameof(ProgressView.ProgressValue))
            {
                SVProgressHUD.ShowProgress(Element.ProgressValue);
            }
        }
    }
}
