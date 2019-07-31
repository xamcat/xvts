using System;
using Android.Widget;
using Android.Views;
using Faucet.Models;

namespace Faucet.Droid.Adapters
{
    public class FishAdapter : BaseAdapter<Fish>
    {
        WeakReference<FishActivity> _owner;

        FishActivity Owner
        {
            get
            {
                if (_owner.TryGetTarget(out FishActivity owner))
                    return owner;
                return null;
            }
        }

        public FishAdapter(FishActivity owner)
        {
            _owner = new WeakReference<FishActivity>(owner);
        }

        public override int Count => Owner.ViewModel.Fish.Count;

        public override Fish this[int position] => Owner.ViewModel.Fish[position];

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? LayoutInflater.FromContext(Owner).Inflate(Resource.Layout.ImageCell, null);

            var fish = Owner.ViewModel.Fish[position];

            var text = view.FindViewById<TextView>(Resource.Id.Title);
            text.Text = fish.Name;

            var imageId = Owner.Resources.GetIdentifier(fish.ImageSource, "drawable", Owner.PackageName);
            var image = view.FindViewById<ImageView>(Resource.Id.Thumbnail);
            image.SetImageResource(imageId);

            return view;
        }
    }
}
