
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MCWeather.Common.Models;
using MCWeather.Common.ViewModels;
using MCWeather.Common.Droid.Views;
using Android.Views;
using System.Threading.Tasks;
using Toolbox.Droid.Helpers;
using Android.Text;
using Android.Text.Style;
using Toolbox.Portable.Mvvm;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Support.V7.App;

using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MCWeather.Droid.Activities
{
    [Activity(Label = "WeatherDetailActivity", ParentActivity = typeof(WeatherListActivity), Theme = "@style/MyTheme")]
    public class WeatherDetailActivity : BaseActivity
    {
        ImageView _backgroundImageView;
        ThermometerView _thermometerView;
        TextView _descriptionLabel;
        TextView _currentTemperatureLabel;
        TextView _lowAndHighTemperatureLabel;

        WeatherDetailViewModel _viewModel;
        public WeatherDetailViewModel ViewModel => _viewModel ?? (_viewModel = new WeatherDetailViewModel());

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitializeView();

            var city = Intent.GetStringExtra("city");
            this.Title = city;
            ViewModel.SetWeatherRequest(new WeatherRequest() { City = new City { Name = city } });

            Task.Run(async () => await ViewModel.InitAsync());
        }

        protected override void OnResume()
        {
            base.OnResume();

            AddBinding(Binding.Create(() => _descriptionLabel.Text == ViewModel.Description, (a) => RunOnUiThread(a)));
            AddBinding(Binding.Create(() => _lowAndHighTemperatureLabel.Text == ViewModel.HighAndLowTemperatureFormatted, (a) => RunOnUiThread(a)));
            AddBinding(Binding.Create(() => _thermometerView.Temperature == ViewModel.CurrentTemperature, (a) => RunOnUiThread(a)));
            AddBinding(Binding.Create(() => Title == ViewModel.CityName, (a) => RunOnUiThread(a)));
            AddBinding(Binding.Create(() => ViewModel.CurrentTemperatureFormatted, HandleWeatherChanged));
            AddBinding(Binding.Create(() => ViewModel.BackgroundPath, HandleBackgroundChanged));
        }

        protected override void OnPause()
        {
            base.OnPause();

            RemoveBindings();
        }

        void InitializeView()
        {
            var relativeLayout = new RelativeLayout(this);
            relativeLayout.LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent
            );

            int nextID = 100;
            var density = this.Resources.DisplayMetrics.Density;

            _backgroundImageView = new ImageView(this);
            var backgroundLayout = new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent
            );
            _backgroundImageView.LayoutParameters = backgroundLayout;
            _backgroundImageView.SetBackgroundColor(Color.LightGray);
            relativeLayout.AddView(_backgroundImageView);

            _lowAndHighTemperatureLabel = new TextView(this);
            _lowAndHighTemperatureLabel.Id = nextID++;
            _lowAndHighTemperatureLabel.SetTextSize(ComplexUnitType.Dip, 20f);
            _lowAndHighTemperatureLabel.SetPadding(
                PixelHelper.DpToPixels(10, density),
                0,
                0,
                PixelHelper.DpToPixels(60, density)
            );
            _lowAndHighTemperatureLabel.SetTextColor(Color.White);
            var lowAndHighTemperatureLayout = new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent
            );
            lowAndHighTemperatureLayout.AddRule(LayoutRules.AlignParentBottom);
            _lowAndHighTemperatureLabel.LayoutParameters = lowAndHighTemperatureLayout;
            relativeLayout.AddView(_lowAndHighTemperatureLabel);

            _currentTemperatureLabel = new TextView(this);
            _currentTemperatureLabel.Id = nextID++;
            _currentTemperatureLabel.SetTextSize(ComplexUnitType.Dip, 50f);
            _currentTemperatureLabel.SetPadding(
                PixelHelper.DpToPixels(10, density),
                0,
                0,
                0
            );
            _currentTemperatureLabel.SetTextColor(Color.White);
            var currentTemperatureLayout = new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent
            );
            currentTemperatureLayout.AddRule(LayoutRules.Above, _lowAndHighTemperatureLabel.Id);
            _currentTemperatureLabel.LayoutParameters = currentTemperatureLayout;
            relativeLayout.AddView(_currentTemperatureLabel);

            _descriptionLabel = new TextView(this);
            _descriptionLabel.Id = nextID++;
            _descriptionLabel.SetTextSize(ComplexUnitType.Dip, 20f);
            _descriptionLabel.SetPadding(
                PixelHelper.DpToPixels(10, density),
                0,
                0,
                0
            );
            _descriptionLabel.SetTextColor(Color.White);
            var descriptionLayout = new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent
            );
            descriptionLayout.AddRule(LayoutRules.Above, _currentTemperatureLabel.Id);
            _descriptionLabel.LayoutParameters = descriptionLayout;
            relativeLayout.AddView(_descriptionLabel);

            _thermometerView = new ThermometerView(this);
            _thermometerView.Id = nextID++;
            _thermometerView.SetPadding(
                PixelHelper.DpToPixels(10, density),
                PixelHelper.DpToPixels(20, density),
                PixelHelper.DpToPixels(10, density),
                PixelHelper.DpToPixels(10, density)
            );
            var thermometerLayout = new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent
            );
            thermometerLayout.AddRule(LayoutRules.Above, _descriptionLabel.Id);
            thermometerLayout.AddRule(LayoutRules.AlignParentTop);
            _thermometerView.LayoutParameters = thermometerLayout;
            relativeLayout.AddView(_thermometerView);

            SetContentView(relativeLayout);
        }

        void HandleWeatherChanged()
        {
            var currentTemp = ViewModel.CurrentTemperatureFormatted;
            var currentTempSpannableString = new SpannableString(currentTemp);

            for (int i = 0; i < currentTemp.Length; i++)
            {
                if (currentTemp[i] == '°')
                {
                    currentTempSpannableString.SetSpan(new RelativeSizeSpan(0.5f), i, currentTemp.Length, 0);
                    break;
                }
            }

            RunOnUiThread(() =>
            {
                _currentTemperatureLabel.TextFormatted = currentTempSpannableString;
            });
        }

        async void HandleBackgroundChanged()
        {
            var imageBitmap = await BitmapFactory.DecodeFileAsync(ViewModel.BackgroundPath);

            RunOnUiThread(() =>
            {
                _backgroundImageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                _backgroundImageView.SetImageBitmap(imageBitmap);
            });
        }
    }
}
