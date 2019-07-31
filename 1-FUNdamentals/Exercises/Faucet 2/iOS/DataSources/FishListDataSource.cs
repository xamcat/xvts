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
        FishTableViewController _owner;

        public FishListDataSource(FishTableViewController owner)
        {
            _owner = owner;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("cell") ?? new UITableViewCell(UITableViewCellStyle.Default, "cell");

            var fish = _owner.ViewModel.Fish[indexPath.Row];

            cell.TextLabel.Text = fish.Name;

            cell.ImageView.Image = UIImage.FromBundle(fish.ImageSource);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _owner.ViewModel.Fish.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var detailViewController = new FishDetailViewController(_owner, _owner.ViewModel.Fish[indexPath.Row]);
            _owner.ShowViewController(detailViewController, this);
        }
    }
}
