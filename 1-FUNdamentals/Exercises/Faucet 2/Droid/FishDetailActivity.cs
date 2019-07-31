using System;
using Android.OS;
using Faucet.ViewModels;
using Android.App;
using Android.Widget;

namespace Faucet.Droid
{
    [Activity(Label = "Detail", Theme = "@style/MyTheme")]
    public class FishDetailActivity : BaseActivity<FishDetailViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var name = Intent.GetStringExtra("name");
            var imageSource = Intent.GetStringExtra("imageSource");

            SetContentView(Resource.Layout.Detail);

            Title = name;

            var imageId = Resources.GetIdentifier(imageSource, "drawable", PackageName);
            var image = FindViewById<ImageView>(Resource.Id.imageView);
            image.SetImageResource(imageId);
        }
    }
}
