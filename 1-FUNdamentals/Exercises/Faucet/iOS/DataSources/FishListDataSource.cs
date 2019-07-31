using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Faucet.iOS.ViewControllers;
using Faucet.Models;
namespace Faucet.iOS.DataSources
{
    public class FishListDataSource : UITableViewSource
    {
        WeakReference<FishTableViewController> _ownerWeak;

        FishTableViewController Owner
        {
            get
            {
                if (_ownerWeak.TryGetTarget(out FishTableViewController owner))
                    return owner;
                return null;
            }
        }

        public FishListDataSource(FishTableViewController owner)
        {
            _ownerWeak = new WeakReference<FishTableViewController>(owner);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("cell") ?? new UITableViewCell(UITableViewCellStyle.Default, "cell");

            var fish = Owner.ViewModel.Fish[indexPath.Row];

            cell.TextLabel.Text = fish.Name;

            cell.ImageView.Image?.Dispose();
            cell.ImageView.Image = null;

            cell.ImageView.Image = UIImage.FromBundle(fish.ImageSource);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Owner.ViewModel.Fish.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var detailViewController = new FishDetailViewController(Owner, Owner.ViewModel.Fish[indexPath.Row]);
            Owner.ShowViewController(detailViewController, this);
        }
    }
}
