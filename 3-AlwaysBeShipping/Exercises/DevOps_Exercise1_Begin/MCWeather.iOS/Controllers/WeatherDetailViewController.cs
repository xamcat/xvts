using System;
using MCWeather.Common.ViewModels;
using UIKit;
using MCWeather.Common.iOS.Views;
using CoreGraphics;
using Foundation;
using CoreText;
using Toolbox.Portable.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace MCWeather.iOS.Controllers
{
    public class WeatherDetailViewController : BaseViewController
    {
        UIImageView _backgroundImageView;
        ThermometerView _thermometerView;
        UILabel _descriptionLabel;
        UILabel _currentTemperatureLabel;
        UILabel _lowAndHighTemperatureLabel;

        WeatherDetailViewModel _viewModel;
        public WeatherDetailViewModel ViewModel => _viewModel ?? (_viewModel = new WeatherDetailViewModel());

        public override void LoadView()
        {
            base.LoadView();

            View.BackgroundColor = UIColor.White;

            _backgroundImageView = new UIImageView();
            _backgroundImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            View.AddSubview(_backgroundImageView);

            _thermometerView = new ThermometerView();
            View.AddSubview(_thermometerView);

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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            AddBinding(Binding.Create(() => _descriptionLabel.Text == ViewModel.Description, (a) => BeginInvokeOnMainThread(a)));
            AddBinding(Binding.Create(() => _lowAndHighTemperatureLabel.Text == ViewModel.HighAndLowTemperatureFormatted, (a) => BeginInvokeOnMainThread(a)));
            AddBinding(Binding.Create(() => _thermometerView.Temperature == ViewModel.CurrentTemperature, (a) => BeginInvokeOnMainThread(a)));
            AddBinding(Binding.Create(() => Title == ViewModel.CityName, (a) => BeginInvokeOnMainThread(a)));
            AddBinding(Binding.Create(() => ViewModel.CurrentTemperatureFormatted, HandleWeatherChanged));
            AddBinding(Binding.Create(() => ViewModel.BackgroundPath, HandleBackgroundChanged));
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            RemoveBindings();
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

            _thermometerView.Frame = new CGRect(
                20,
                84,
                bounds.Width - 40,
                (bounds.Height / 2) - 40
            );

            _descriptionLabel.Frame = new CGRect(
                20,
                _thermometerView.Frame.GetMaxY() + 10,
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

        void HandleWeatherChanged()
        {
            var currentTemp = ViewModel.CurrentTemperatureFormatted;
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

            InvokeOnMainThread(() =>
            {
                _currentTemperatureLabel.AttributedText = currentTempAttributedText;
            });
        }

        void HandleBackgroundChanged()
        {
            InvokeOnMainThread(() =>
            {
                _backgroundImageView.Image = UIImage.FromFile(ViewModel.BackgroundPath);
            });
        }
    }
}
