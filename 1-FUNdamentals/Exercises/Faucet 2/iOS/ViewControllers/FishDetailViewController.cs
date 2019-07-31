using System;
using Faucet.ViewModels;
using UIKit;
using CoreGraphics;
using Faucet.Models;

namespace Faucet.iOS.ViewControllers
{
    public class FishDetailViewController : BaseViewController<FishDetailViewModel>
    {
        Fish _fish;

        UINavigationBar _navigationBar;
        UINavigationItem _navigationItem;

        UIImageView _imageView;

        UIViewController _owner;

        public FishDetailViewController(UIViewController owner, Fish fish)
        {
            _owner = owner;
            _fish = fish;
        }

        public override void LoadView()
        {
            base.LoadView();

            _navigationBar = new UINavigationBar();
            _navigationItem = new UINavigationItem();
            _navigationItem.Title = _fish.Name;

            _navigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
            {
                _owner.DismissViewController(true, null);
            }), false);

            _navigationBar.Items = new UINavigationItem[]{
                _navigationItem
            };

            View.AddSubview(_navigationBar);

            _imageView = new UIImageView(UIImage.FromBundle(_fish.ImageSource));
            _imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            View.AddSubview(_imageView);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            var bounds = View.Bounds;

            _navigationBar.Frame = new CGRect(
                0,
                0,
                bounds.Width,
                64
            );

            _imageView.Frame = new CGRect(
                0,
                64,
                bounds.Width,
                bounds.Height - 64
            );
        }
    }
}
