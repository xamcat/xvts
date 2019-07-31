using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreLocation;
using Toolbox.Portable.Services;

namespace Toolbox.iOS.Services
{
    public class LocationService : ILocationService
    {
        private CLLocationManager locationManager;

        public bool HasLocationPermission
        {
            get
            {
                return CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse ||
                CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways;
            }
        }

        public Task<bool> RequestPermissionAsync()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            EventHandler<CLAuthorizationChangedEventArgs> authorizationChanged = null;

            try
            {
                if (CLLocationManager.Status != CLAuthorizationStatus.NotDetermined && !this.HasLocationPermission)
                {
                    tcs.SetResult(false);
                }
                else
                {
                    authorizationChanged = (sender, e) =>
                    {
                        CLAuthorizationStatus update = e.Status;
                        this.locationManager.AuthorizationChanged -= authorizationChanged;

                        if (update == CLAuthorizationStatus.AuthorizedWhenInUse || update == CLAuthorizationStatus.AuthorizedAlways)
                        {
                            tcs.SetResult(true);
                        }
                        else
                        {
                            tcs.SetResult(false);
                        }
                    };

                    this.locationManager.AuthorizationChanged += authorizationChanged;
                    this.locationManager.RequestWhenInUseAuthorization();
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                this.locationManager.AuthorizationChanged -= authorizationChanged;
                this.locationManager.StopUpdatingLocation();
            }

            return tcs.Task;
        }

        public LocationService()
        {
            this.locationManager = new CLLocationManager
            {
                DesiredAccuracy = CLLocation.AccuracyKilometer,
                PausesLocationUpdatesAutomatically = false
            };
        }

        public async Task<LocationInfo> GetCurrentLocationAsync()
        {
            TaskCompletionSource<LocationInfo> tcs = new TaskCompletionSource<LocationInfo>();
            EventHandler<CLLocationsUpdatedEventArgs> locationsUpdated = null;

            try
            {
                if (!this.HasLocationPermission)
                {
                    if (await this.RequestPermissionAsync() != true)
                    {
                        return null;
                    }
                }

                locationsUpdated = (sender, e) =>
                {
                    CLLocation update = e.Locations.FirstOrDefault();
                    locationManager.LocationsUpdated -= locationsUpdated;
                    locationManager.StopUpdatingLocation();

                    if (update?.Coordinate != null)
                    {
                        tcs.SetResult(new LocationInfo(update.Coordinate.Latitude, update.Coordinate.Longitude));
                    }
                    else
                    {
                        tcs.SetException(new NullReferenceException());
                    }
                };

                locationManager.LocationsUpdated += locationsUpdated;
                locationManager.StartUpdatingLocation();
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                locationManager.LocationsUpdated -= locationsUpdated;
                locationManager.StopUpdatingLocation();
            }

            await tcs.Task;

            return tcs.Task.Result;
        }

        public async Task<string> GetCityNameFromLocationAsync(LocationInfo location)
        {
            string city = string.Empty;

            using (var geocoder = new CLGeocoder())
            {
                var addresses = await geocoder.ReverseGeocodeLocationAsync(new CLLocation(location.Latitude, location.Longitude));
                city = addresses.FirstOrDefault()?.Locality;
            }

            return city;
        }
    }
}