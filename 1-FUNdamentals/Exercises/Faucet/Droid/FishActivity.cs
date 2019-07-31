using Android.App;
using Android.Widget;
using Android.OS;
using Faucet.Models;
using Faucet.ViewModels;
using Faucet.Droid.Adapters;
using System.Collections.Generic;
using System.Linq;
using Android.Content;

namespace Faucet.Droid
{
    [Activity(Label = "Faucet", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/MyTheme")]
    public class FishActivity : BaseActivity<FishListViewModel>
    {
        ListView _listView;
        FishAdapter _listAdapter;

        public FishActivity()
        {
            ViewModel.ReloadAction = () => RunOnUiThread(() => UpdateListView());
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            _listView = FindViewById<ListView>(Resource.Id.listView);
            _listView.Adapter = _listAdapter = new FishAdapter(this);

            _listView.ItemClick += OnListViewItemClick;
        }

        protected override void OnDestroy()
        {
            _listView.ItemClick -= OnListViewItemClick;

            base.OnDestroy();
        }

        void OnListViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var fish = ViewModel.Fish[e.Position];
            var intent = new Intent(this, typeof(FishDetailActivity));
            intent.PutExtra("name", fish.Name);
            intent.PutExtra("imageSource", fish.ImageSource);

            StartActivity(intent);
        }


        void UpdateListView()
        {
            _listAdapter.NotifyDataSetChanged();
        }
    }
}

