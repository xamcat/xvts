using System;
using Foundation;
using MCWeather.Common.ViewModels;
using UIKit;
using CoreGraphics;
using MCWeather.Common.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using Toolbox.Portable.Mvvm;

namespace MCWeather.iOS.Controllers
{
    public class WeatherListViewController : BaseViewController, IUITableViewDelegate, IUITableViewDataSource
    {
        const string CellId = "WeatherListCell";

        UITableView _tableView;
        UIRefreshControl _refreshControl;

        WeatherListViewModel _viewModel;
        public WeatherListViewModel ViewModel => _viewModel ?? (_viewModel = new WeatherListViewModel());

        public override void LoadView()
        {
            base.LoadView();

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

            ViewModel.ReloadAction = () => BeginInvokeOnMainThread(_tableView.ReloadData);

            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(
                    "Add",
                    UIBarButtonItemStyle.Plain,
                    (sender, args) =>
                    {
                        ViewModel.AddFavoriteCommand.Execute(null);
                    }),
                true);

            Task.Run(async () =>
            {
                try
                {
                    await ViewModel.InitAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });

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

            AddBinding(Binding.Create(() => ViewModel.IsRefreshing, OnRefreshComplete));

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                _tableView.RefreshControl.ValueChanged += OnRefreshControlValueChanged;
            else
                _refreshControl.ValueChanged += OnRefreshControlValueChanged;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            RemoveBindings();

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                _tableView.RefreshControl.ValueChanged -= OnRefreshControlValueChanged;
            else
                _refreshControl.ValueChanged -= OnRefreshControlValueChanged;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var weatherRequest = ViewModel.WeatherRequests[indexPath.Row];

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
            return ViewModel.WeatherRequests?.Count ?? 0;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var weatherRequest = ViewModel.WeatherRequests[indexPath.Row];

            var detailViewController = new WeatherDetailViewController();
            detailViewController.ViewModel.SetWeatherRequest(weatherRequest);

            NavigationController.PushViewController(detailViewController, true);
        }

        void OnRefreshControlValueChanged(object sender, EventArgs e)
        {
            ViewModel.RefreshCommand.Execute(null);
        }

        void OnRefreshComplete()
        {
            if (ViewModel.IsRefreshing) return;

            InvokeOnMainThread(() =>
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                    _tableView.RefreshControl.EndRefreshing();
                else
                    _refreshControl.EndRefreshing();
            });
        }
    }
}
