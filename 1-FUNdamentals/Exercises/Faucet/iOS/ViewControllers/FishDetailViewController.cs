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
        UIBarButtonItem _doneButton;

        UIImageView _imageView;

        WeakReference<UIViewController> _ownerWeak;

        UIViewController Owner
        {
            get
            {
                if (_ownerWeak.TryGetTarget(out UIViewController owner))
                    return owner;
                return null;
            }
        }

        public FishDetailViewController(UIViewController owner, Fish fish)
        {
            _ownerWeak = new WeakReference<UIViewController>(owner);
            _fish = fish;
        }

        public override void LoadView()
        {
            base.LoadView();

            _navigationBar = new UINavigationBar();
            _navigationItem = new UINavigationItem();
            _navigationItem.Title = _fish.Name;

            _doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);

            _navigationItem.RightBarButtonItem = _doneButton;

            _navigationBar.Items = new UINavigationItem[]{
                _navigationItem
            };

            View.AddSubview(_navigationBar);

            _imageView = new UIImageView(UIImage.FromBundle(_fish.ImageSource));
            _imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            View.AddSubview(_imageView);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _doneButton.Clicked += _doneButton_Clicked;
        }

        public override void ViewDidDisappear(bool animated)
        {
            _doneButton.Clicked -= _doneButton_Clicked;

            base.ViewDidDisappear(animated);
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

        void _doneButton_Clicked(object sender, EventArgs e)
        {
            Owner.DismissViewController(true, null);

            // Note: you shouldn't manually call GC.Collect in a production app;
            // this is meant solely for profiling purposes.
            GC.Collect();
        }
    }
}
