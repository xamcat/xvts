using System;
using UIKit;
using CoreGraphics;
using CoreAnimation;
using Foundation;
using System.ComponentModel;

namespace MCWeather.Common.iOS.Views
{
    [Register("ThermometerView"), DesignTimeVisible(true)]
    public class ThermometerView : UIView
    {
        private UIView _needleHolder;
        private UIImageView _backgroundImage;
        private UIImageView _needleImage;
        private bool _isImperial = false;
        private int? _temperature = null;

        public Action TemperatureChanged;

        public bool IsImperial
        {
            get { return _isImperial; }
            set
            {
                if (_isImperial != value)
                {
                    _isImperial = value;
                    UpdateUnits();
                }
            }
        }

        public int? Temperature
        {
            get { return _temperature; }
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    UpdateTemperature();
                }
            }
        }

        public ThermometerView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        public ThermometerView()
        {
            Initialize();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var bounds = Bounds;

            var maxEdge = Math.Min(bounds.Width, bounds.Height);

            _backgroundImage.Frame = new CGRect(
                bounds.GetMidX() - (maxEdge / 2),
                bounds.GetMidY() - (maxEdge / 2),
                maxEdge,
                maxEdge
            );

            _needleHolder.Frame = new CGRect(
                bounds.GetMidX() - (maxEdge / 2),
                bounds.GetMidY() - (maxEdge / 2),
                maxEdge,
                maxEdge
            );

            _needleImage.Frame = new CGRect(
                0,
                0,
                _needleHolder.Bounds.Width,
                _needleHolder.Bounds.Height
            );
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Clear;

            _backgroundImage = new UIImageView(UIImage.FromBundle("fahrenheit"), UIImage.FromBundle("celsius"));
            _backgroundImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            AddSubview(_backgroundImage);

            _needleHolder = new UIView();
            _needleImage = new UIImageView(UIImage.FromBundle("needle"));
            _needleImage.ContentMode = UIViewContentMode.ScaleAspectFit;

            _needleHolder.AddSubview(_needleImage);
            AddSubview(_needleHolder);
        }

        void UpdateTemperature()
        {
            var rotation = IsImperial ? 2 * Temperature : Temperature;

            var radians = (rotation * Math.PI) / 180.00;

            UIView.AnimateNotify(0.4f, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                _needleImage.Transform = CGAffineTransform.MakeRotation((nfloat)radians);
            }, null);
        }

        void UpdateUnits()
        {
            _backgroundImage.Highlighted = IsImperial;

            Temperature = IsImperial ? Temperature / 2 : Temperature * 2;
            TemperatureChanged?.Invoke();
        }
    }
}
