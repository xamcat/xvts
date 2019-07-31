using System;
using Android.App;
using Android.Content;
using Android.Support.V7.App;
using MCWeather.Forms.Views;
using Android.Views;
using Xamarin.Forms.Platform.Android;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;

namespace MCWeather.Droid.Activities
{
    [Activity(Label = "MCWeather", MainLauncher = true, Theme = "@style/MyTheme", Icon = "@mipmap/icon")]
    public class WeatherListFormsActivity : AppCompatActivity
    {
        Toolbar _toolbar;
        WeatherListPage _listPage;
        Android.Support.V4.App.Fragment _listPageFragment;

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            InitializeView();
        }

        protected override void OnResume()
        {
            base.OnResume();

            _listPage.WeatherSelected += ListPage_WeatherSelected;
        }

        protected override void OnPause()
        {
            base.OnPause();

            _listPage.WeatherSelected -= ListPage_WeatherSelected;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add("Add");
            item.SetShowAsAction(ShowAsAction.Always);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            _listPage.ViewModel.AddFavoriteCommand.Execute(null);
            return true;
        }

        void InitializeView()
        {
            var linearLayout = new LinearLayout(this);
            linearLayout.LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent
            );
            linearLayout.Orientation = Orientation.Vertical;

            _toolbar = new Toolbar(this);
            _toolbar.Title = "Weather";
            _toolbar.SetTitleTextColor(Android.Graphics.Color.DodgerBlue);
            _toolbar.SetBackgroundColor(Android.Graphics.Color.WhiteSmoke);
            _toolbar.LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent
            );
            linearLayout.AddView(_toolbar);
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(true);

            var frameLayout = new FrameLayout(this);
            frameLayout.LayoutParameters = new FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent
            );
            frameLayout.Id = new Random().Next(int.MaxValue);
            linearLayout.AddView(frameLayout);

            _listPage = new WeatherListPage();
            _listPageFragment = _listPage.CreateSupportFragment(this);

            var ft = SupportFragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Replace(frameLayout.Id, _listPageFragment);
            ft.Commit();

            SetContentView(linearLayout);
        }

        void ListPage_WeatherSelected(Common.Models.WeatherRequest obj)
        {
            var intent = new Intent(this, typeof(WeatherDetailActivity));
            intent.PutExtra("city", obj.ToString());
            StartActivity(intent);
        }
    }
}
