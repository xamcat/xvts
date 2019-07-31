using System;
using Android.Widget;
using Android.Views;
using Faucet.Models;

namespace Faucet.Droid.Adapters
{
    public class FishAdapter : BaseAdapter<Fish>
    {
        FishActivity _owner;

        public FishAdapter(FishActivity owner)
        {
            _owner = owner;
        }

        public override int Count => _owner.ViewModel.Fish.Count;

        public override Fish this[int position] => _owner.ViewModel.Fish[position];

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? LayoutInflater.FromContext(_owner).Inflate(Resource.Layout.ImageCell, null);

            var fish = _owner.ViewModel.Fish[position];

            var text = view.FindViewById<TextView>(Resource.Id.Title);
            text.Text = fish.Name;

            var imageId = _owner.Resources.GetIdentifier(fish.ImageSource, "drawable", _owner.PackageName);
            var image = view.FindViewById<ImageView>(Resource.Id.Thumbnail);
            image.SetImageResource(imageId);

            return view;
        }
    }
}
