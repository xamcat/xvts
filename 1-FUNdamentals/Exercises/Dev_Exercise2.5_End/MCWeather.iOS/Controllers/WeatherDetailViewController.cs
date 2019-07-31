using System;
using UIKit;
using CoreGraphics;
using Foundation;
using CoreText;

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
            base.LoadView();

            View.BackgroundColor = UIColor.White;

            Title = "City";

            _backgroundImageView = new UIImageView();
            _backgroundImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            _backgroundImageView.BackgroundColor = UIColor.LightGray;
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

            _descriptionLabel.Text = "Cloudy";
            _lowAndHighTemperatureLabel.Text = "High: 65 °F Low: 62 °F";

            var currentTemp = "64 °F";
            var currentTempAttributedText = new NSMutableAttributedString(currentTemp);

            for (int i = 0; i < currentTemp.Length; i++)
            {
                if (currentTemp[i] == '°')
                {
                    currentTempAttributedText.AddAttribute(CTStringAttributeKey.Font, UIFont.SystemFontOfSize(56), new NSRange(0, i - 1));
                    currentTempAttributedText.AddAttribute(CTStringAttributeKey.Font, UIFont.SystemFontOfSize(24), new NSRange(i, currentTemp.Length - i));
                    break;
                }
            }

            _currentTemperatureLabel.AttributedText = currentTempAttributedText;
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
