
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Android.Views;
using Toolbox.Droid.Helpers;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Support.V7.App;

namespace MCWeather.Droid.Activities
{
    [Activity(Label = "WeatherDetailActivity", ParentActivity = typeof(WeatherListActivity), Theme = "@style/MyTheme")]
    public class WeatherDetailActivity : AppCompatActivity
    {
        ImageView _backgroundImageView;
        TextView _descriptionLabel;
        TextView _currentTemperatureLabel;
        TextView _lowAndHighTemperatureLabel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitializeView();

            this.Title = "City";
            _descriptionLabel.Text = "Cloudy";
            _lowAndHighTemperatureLabel.Text = "High: 65 °F Low: 62 °F";

            var currentTemp = "64 °F";
            var currentTempSpannableString = new SpannableString(currentTemp);
            for (int i = 0; i < currentTemp.Length; i++)
            {
                if (currentTemp[i] == '°')
                {
                    currentTempSpannableString.SetSpan(new RelativeSizeSpan(0.5f), i, currentTemp.Length, 0);
                    break;
                }
            }

            _currentTemperatureLabel.TextFormatted = currentTempSpannableString;
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

            SetContentView(relativeLayout);
        }
    }
}
