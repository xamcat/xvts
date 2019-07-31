
using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MCWeather.Common.ViewModels;
using MCWeather.Common.Models;
using System.Threading.Tasks;
using MCWeather.Droid.Adapters;
using Android.Support.V4.Widget;
using Toolbox.Portable.Mvvm;
using System.Collections.Generic;
using Android.Support.V7.App;

using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MCWeather.Droid.Activities
{
    [Activity(Label = "MCWeather", MainLauncher = true, Theme = "@style/MyTheme", Icon = "@mipmap/icon")]
    public class WeatherListActivity : AppCompatActivity
    {
        SwipeRefreshLayout _refreshLayout;
        Toolbar _toolbar;
        ListView _listView;
        WeatherListAdapter _listAdapter;

        List<WeatherRequest> _requests;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // TODO: Hook up data from view model to Activity

            base.OnCreate(savedInstanceState);

            InitializeView();

            _requests = new List<WeatherRequest>
            {
                new WeatherRequest { City = new City { Name="Current Location" } },
                new WeatherRequest { City = new City { Name="Bellevue" } },
                new WeatherRequest { City = new City { Name="San Francisco" } },
                new WeatherRequest { City = new City { Name="London" } }
            };

            _listAdapter = new WeatherListAdapter(this, _requests);
            _listView.Adapter = _listAdapter;
        }

        protected override void OnResume()
        {
            base.OnResume();

            _listView.ItemClick += OnListViewItemClick;
            _refreshLayout.Refresh += OnRefresh;
        }

        protected override void OnPause()
        {
            base.OnPause();

            _refreshLayout.Refresh -= OnRefresh;
            _listView.ItemClick -= OnListViewItemClick;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add("Add");
            item.SetShowAsAction(ShowAsAction.Always);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // TODO: Hook into view model actions
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

            _refreshLayout = new SwipeRefreshLayout(this);
            _refreshLayout.LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent
            );

            _listView = new ListView(this);
            _listView.LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent
            );
            _listView.ChoiceMode = ChoiceMode.Single;
            _refreshLayout.AddView(_listView);
            linearLayout.AddView(_refreshLayout);

            SetContentView(linearLayout);
        }

        void OnListViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(WeatherDetailActivity));
            var city = _requests[e.Position];
            intent.PutExtra("city", city.ToString());
            StartActivity(intent);
        }

        void OnRefresh(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Refresh began");
        }
    }
}

