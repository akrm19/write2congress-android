using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;

namespace Write2Congress.Droid.Code
{
    public static class GeoHelper
    {
        private static Logger _logger = new Logger("GeoHelper");

        public static bool IsAddressInUs(Address address)
        {
            return address != null
                && !string.IsNullOrWhiteSpace(address.CountryCode)
                && address.CountryCode.ToLower().Equals("us");
        }

        public static Location GetCurrentLocation(LocationManager locationManager = null) 
        {
            Location location = null;

            //OnCreate is a good place to get a reference to the LocationManager
            if (locationManager == null)
                locationManager = GetLocationManager();

            var locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;

            var bestLocationProvider = locationManager.GetBestProvider(locationCriteria, true);

            if (bestLocationProvider != null)
                location = locationManager.GetLastKnownLocation(bestLocationProvider);
            else
                _logger.Warn("Unable to get LocationManager's BestProvider");

            return location;
        }

        public static Address GetCurrentAddress(LocationManager locationManager = null)
        {
            if (locationManager == null)
                locationManager = GetLocationManager();

            var currentLocation = GetCurrentLocation(locationManager);

            return GetAddressFromLocation(currentLocation);
        }

        public static LocationManager GetLocationManager()
        {
            return Application.Context.GetSystemService(Context.LocationService) as LocationManager;
        }

        public static LocationManager GetLocationManager(Context context)
        {
            return context.GetSystemService(Context.LocationService) as LocationManager;
        }

        public static Address GetAddressFromLocation(Location loc)
        {
            if(loc == null)
            {
                _logger.Error("Location is null. Cannot retrive address.");
                return null;
            }

            return GetAddressFromCoordinates(loc.Latitude, loc.Longitude);
        }

        public static Address GetAddressFromCoordinates(double latitude, double longitude)
        {
            var location = GetGeoCoder().GetFromLocation(latitude, longitude, 1);

            return location.FirstOrDefault();
        }

        public static string GetZipFromFromCoordinates(double latitude, double longitude)
        {
            var address = GetAddressFromCoordinates(latitude, longitude);

            return address == null
                ? string.Empty
                : address.PostalCode;
        }

        public static string GetStateFromCoordinates(double latitude, double longitude)
        {
            var address = GetAddressFromCoordinates(latitude, longitude);

            return address == null
                ? string.Empty
                : address.AdminArea;
        }

        #region Helper Methods
        private static Geocoder GetGeoCoder()
        {
            return new Geocoder(Application.Context);
        }
        #endregion
    }
}