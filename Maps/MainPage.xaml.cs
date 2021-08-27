using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps
{
    public partial class MainPage : ContentPage
    {
        private bool hasLocationPermission;

        [Obsolete]
        public MainPage()
        {

            InitializeComponent();
            GetPermission();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var geoLocator = CrossGeolocator.Current;
            geoLocator.PositionChanged += GeoLocator_PositionChanged;

        }

        private void GeoLocator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            GetUserlocation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }

        [Obsolete]
        private async void GetPermission()
        {

            var Status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            if (Status != PermissionStatus.Granted)
            {

                await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse);
            }
            var result = CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);

            if (result.Result.ContainsKey(Permission.LocationWhenInUse))
            {
                Status = result.Result[Permission.LocationWhenInUse];
            }
            if (Status == PermissionStatus.Granted)
            {
                locationMap.IsShowingUser = true;
                hasLocationPermission = true;
            }
            else
            {
                hasLocationPermission = false;
                await DisplayAlert("Please turn on the Location", "", "Ok");
            }

        }

        private void GetUserlocation()
        {
            if (hasLocationPermission)
            {
                var geoLocator = CrossGeolocator.Current;
                var postion = geoLocator.GetPositionAsync().Result;
                MoveMap(postion);
            }
        }

        private void MoveMap(Plugin.Geolocator.Abstractions.Position position)
        {

            var center = new Xamarin.Forms.Maps.Position(position.Latitude, position.Latitude);
            var span = new MapSpan(center, 1, 1);

            locationMap.MoveToRegion(span);


        }
    }
}