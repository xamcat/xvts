using System;
using Faucet.ViewModels;
using UIKit;
using Faucet.iOS.DataSources;
using CoreGraphics;
using System.Linq;
using System.Collections.Generic;
using Faucet.Models;

namespace Faucet.iOS.ViewControllers
{
    public class FishTableViewController : BaseViewController<FishListViewModel>
    {
        UITableView _tableView;
        FishListDataSource _tableSource;

        public override void LoadView()
        {
            base.LoadView();

            _tableView = new UITableView();
            _tableView.Source = _tableSource = new FishListDataSource(this);
            _tableView.RowHeight = 80;
            View.AddSubview(_tableView);

            ViewModel.ReloadAction = () => BeginInvokeOnMainThread(() => UpdateTableView());
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            var bounds = View.Bounds;

            _tableView.Frame = new CGRect(
                bounds.GetMinX(),
                bounds.GetMinY() + 20,
                bounds.Width,
                bounds.Height - 20
            );
        }

        void UpdateTableView()
        {
            _tableView.ReloadData();
        }
    }
}
