
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MCWeather.Common.Models;
using MCWeather.Common.ViewModels;
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
    public class WeatherDetailActivity : AppCompatActivity
    {
        ImageView _backgroundImageView;
        TextView _descriptionLabel;
        TextView _currentTemperatureLabel;
        TextView _lowAndHighTemperatureLabel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // TODO: Hook up data from view model to Activity

            base.OnCreate(savedInstanceState);

            InitializeView();

            var city = Intent.GetStringExtra("city");
            this.Title = city;
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
