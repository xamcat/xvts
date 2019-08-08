using System;
using Android.Content;
using Android.Views;
using NativeBindings;
using NativeBindings.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Com.Github.Guilhe.Views;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ProgressView), typeof(ProgressViewRenderer))]
namespace NativeBindings.Droid
{
    public class ProgressViewRenderer : ViewRenderer<ProgressView, CircularProgressView>
    {
        private CircularProgressView _progressView;

        private readonly Context _context;

        public ProgressViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ProgressView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _progressView = new CircularProgressView(_context);
                SetNativeControl(_progressView);

                //Set values here
                //_progressView.SetBackgroundColor(Element.BackgroundColor.ToAndroid());

                _progressView.SetMinimumWidth(100);
                _progressView.SetMinimumHeight(100);
                _progressView.ProgressStrokeThickness = 10.0f;
                _progressView.ShadowEnabled = true;
                _progressView.ProgressRounded = true;
                _progressView.BackgroundAlphaEnabled = true;

                if (Element.IsDisplayed)
                {
                    _progressView.Visibility = ViewStates.Visible;
                }
                else
                {
                    _progressView.Visibility = ViewStates.Invisible;
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
                    _progressView.Visibility = ViewStates.Visible;
                }
                else
                {
                    _progressView.Visibility = ViewStates.Invisible;
                }
            }
            if (e.PropertyName == nameof(ProgressView.ProgressValue))
            {
                _progressView.SetProgress(Element.ProgressValue * 100, animate: true);
            }
        }
    }
}
