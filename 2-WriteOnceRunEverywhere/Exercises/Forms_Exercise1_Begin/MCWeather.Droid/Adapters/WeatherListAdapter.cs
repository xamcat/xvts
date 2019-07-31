using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.App;
using MCWeather.Common.Models;
namespace MCWeather.Droid.Adapters
{
    public class WeatherListAdapter : BaseAdapter<WeatherRequest>
    {
        Activity _activity;

        public WeatherListAdapter(Activity activity, List<WeatherRequest> weatherRequests = null)
        {
            _activity = activity;
            WeatherRequests = weatherRequests;
        }

        public List<WeatherRequest> WeatherRequests { get; set; }

        public override WeatherRequest this[int position] => WeatherRequests[position];

        public override int Count => WeatherRequests.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView
                ?? _activity.LayoutInflater.Inflate(
                    Android.Resource.Layout.SimpleListItem1,
                    parent,
                    false);

            var title = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            var weatherRequest = WeatherRequests[position];

            if (weatherRequest is LocationWeatherRequest)
            {
                title.Text = "Current Location";
            }
            else
            {
                title.Text = weatherRequest.City.Name;
            }

            return view;
        }
    }
}
