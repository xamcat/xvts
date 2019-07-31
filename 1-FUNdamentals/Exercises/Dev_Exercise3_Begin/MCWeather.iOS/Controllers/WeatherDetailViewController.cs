using System;
using MCWeather.Common.ViewModels;
using UIKit;
using CoreGraphics;
using Foundation;
using CoreText;
using Toolbox.Portable.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace MCWeather.iOS.Controllers
{
    public class WeatherDetailViewController : UIViewController
    {
        UIImageView _backgroundImageView;
        UILabel _descriptionLabel;
        UILabel _currentTemperatureLabel;
        UILabel _lowAndHighTemperatureLabel;

        public override void LoadView()
        {
            // TODO: Hook up data from view model to View Controller

            base.LoadView();

            View.BackgroundColor = UIColor.White;

            _backgroundImageView = new UIImageView();
            _backgroundImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            View.AddSubview(_backgroundImageView);

            _descriptionLabel = new UILabel
            {
                TextColor = UIColor.White,
                Font = UIFont.SystemFontOfSize(24f),
                ShadowOffset = new CGSize(0, -1),
                ShadowColor = UIColor.DarkGray
            };
            View.AddSubview(_descriptionLabel);

            _currentTemperatureLabel = new UILabel()
            {
                TextColor = UIColor.White,
                ShadowOffset = new CGSize(0, -1),
                ShadowColor = UIColor.DarkGray
            };
            View.AddSubview(_currentTemperatureLabel);

            _lowAndHighTemperatureLabel = new UILabel()
            {
                TextColor = UIColor.White,
                Font = UIFont.SystemFontOfSize(24f),
                ShadowOffset = new CGSize(0, -1),
                ShadowColor = UIColor.DarkGray,
            };
            View.AddSubview(_lowAndHighTemperatureLabel);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            var bounds = View.Bounds;

            _backgroundImageView.Frame = new CGRect(
                0,
                64,
                bounds.Width,
                bounds.Height - 64
            );

            _descriptionLabel.Frame = new CGRect(
                20,
                bounds.GetMidY() + 10,
                bounds.Width - 40,
                30
            );

            _currentTemperatureLabel.Frame = new CGRect(
                20,
                _descriptionLabel.Frame.GetMaxY() + 5,
                bounds.Width - 40,
                60
            );

            _lowAndHighTemperatureLabel.Frame = new CGRect(
                20,
                _currentTemperatureLabel.Frame.GetMaxY() + 5,
                bounds.Width - 40,
                30
            );
        }
    }
}
