using System;
using Foundation;
using UIKit;
using CoreGraphics;
using MCWeather.Common.Models;
using System.Diagnostics;
using System.Collections.Generic;

namespace MCWeather.iOS.Controllers
{
    public class WeatherListViewController : UIViewController, IUITableViewDelegate, IUITableViewDataSource
    {
        const string CellId = "WeatherListCell";

        UITableView _tableView;
        UIRefreshControl _refreshControl;

        List<WeatherRequest> _requests;

        public override void LoadView()
        {
            base.LoadView();

            _requests = new List<WeatherRequest>
            {
                new WeatherRequest { City = new City { Name="Current Location" } },
                new WeatherRequest { City = new City { Name="Bellevue" } },
                new WeatherRequest { City = new City { Name="San Francisco" } },
                new WeatherRequest { City = new City { Name="London" } }
            };

            View.BackgroundColor = UIColor.White;

            _tableView = new UITableView();
            _tableView.WeakDataSource = this;
            _tableView.WeakDelegate = this;

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                _tableView.RefreshControl = new UIRefreshControl();
            }
            else
            {
                _refreshControl = new UIRefreshControl();
                _tableView.AddSubview(_refreshControl);
            }

            View.AddSubview(_tableView);

            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(
                    "Add",
                    UIBarButtonItemStyle.Plain,
                    (sender, args) => { }),
                true);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            var bounds = View.Bounds;

            _tableView.Frame = new CGRect(
                0,
                0,
                bounds.Width,
                bounds.Height
            );
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                _tableView.RefreshControl.ValueChanged += OnRefreshControlValueChanged;
            else
                _refreshControl.ValueChanged += OnRefreshControlValueChanged;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                _tableView.RefreshControl.ValueChanged -= OnRefreshControlValueChanged;
            else
                _refreshControl.ValueChanged -= OnRefreshControlValueChanged;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var weatherRequest = _requests[indexPath.Row];

            var cell = tableView.DequeueReusableCell(CellId) ?? new UITableViewCell(UITableViewCellStyle.Default, CellId);

            if (weatherRequest is LocationWeatherRequest)
            {
                cell.TextLabel.Text = "Current Location";
            }
            else
            {
                cell.TextLabel.Text = weatherRequest.City.Name;
            }

            return cell;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return _requests?.Count ?? 0;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var detailViewController = new WeatherDetailViewController();

            NavigationController.PushViewController(detailViewController, true);
        }

        void OnRefreshControlValueChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Refresh began");
        }
    }
}
