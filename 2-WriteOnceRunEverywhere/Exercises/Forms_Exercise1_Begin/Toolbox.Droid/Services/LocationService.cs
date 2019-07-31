using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Toolbox.Portable.Services;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Util;
using Android.Gms.Location;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace Toolbox.Droid.Services
{
    public class LocationService : Java.Lang.Object, ILocationService, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private Activity _activity;
        private GoogleApiClient _apiClient;

        private event EventHandler apiClientConnected;

        public LocationService(Activity activity)
        {
            _activity = activity;

            if (!IsGooglePlayServicesInstalled()) return;

            _apiClient = new GoogleApiClient
                .Builder(activity, this, this)
                .AddApi(LocationServices.API)
                .Build();
        }

        public bool HasLocationPermission
        {
            get
            {
                foreach (var permission in new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation })
                {
                    if (ContextCompat.CheckSelfPermission(_activity, permission) == Permission.Denied)
                        return false;
                }

                return true;
            }
        }

        public Task<bool> RequestPermissionAsync()
        {
            _activity.RunOnUiThread(() =>
            {
                ActivityCompat.RequestPermissions(_activity, new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 0);
            });

            return Task.FromResult(this.HasLocationPermission);
        }

        public async Task<LocationInfo> GetCurrentLocationAsync()
        {
            var tcs = new TaskCompletionSource<LocationInfo>();
            EventHandler onApiClientConnected = null;

            try
            {
                // Bail out if Google Play Services aren't available
                if (_apiClient == null)
                    return null;

                if (!this.HasLocationPermission)
                {
                    if (await this.RequestPermissionAsync() != true)
                    {
                        return null;
                    }
                }

                _apiClient.Connect();

                if (_apiClient.IsConnected)
                {
                    var location = LocationServices.FusedLocationApi.GetLastLocation(_apiClient);
                    if (location != null)
                    {
                        var locationInfo = new LocationInfo(location.Latitude, location.Longitude);
                        tcs.TrySetResult(locationInfo);
                    }
                    else
                    {
                        tcs.TrySetException(new NullReferenceException());
                    }
                }
                else
                {
                    onApiClientConnected = (sender, e) =>
                    {
                        try
                        {
                            apiClientConnected -= onApiClientConnected;

                            var location = LocationServices.FusedLocationApi.GetLastLocation(_apiClient);
                            if (location != null)
                            {
                                var locationInfo = new LocationInfo(location.Latitude, location.Longitude);
                                tcs.TrySetResult(locationInfo);
                            }
                            else
                            {
                                tcs.TrySetException(new NullReferenceException());
                            }
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                        }
                    };

                    apiClientConnected += onApiClientConnected;
                }
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task.Result;
        }

        public async Task<string> GetCityNameFromLocationAsync(LocationInfo location)
        {
            string city = string.Empty;

            using (var geocoder = new Geocoder(_activity))
            {
                var addresses = await geocoder.GetFromLocationAsync(location.Latitude, location.Longitude, 1);
                city = addresses.FirstOrDefault()?.Locality;
            }

            return city;
        }

        #region  GoogleApiClient.IConnectionCallbacks and GoogleApiClient.IOnConnectionFailedListener Implementation

        public void OnConnected(Bundle connectionHint)
        {
            apiClientConnected?.Invoke(this, EventArgs.Empty);
            Log.Info("LocationService", "Now connected to client");
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Log.Info("LocationService", "Connection failed");
        }

        public void OnConnectionSuspended(int cause)
        {
        }

        #endregion

        private bool IsGooglePlayServicesInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(_activity);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info("LocationService", "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error("LocationService", "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);
            }
            return false;
        }
    }
}