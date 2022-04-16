using GeocachingTool.Resources;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompassPage : ContentPage
    {
        /// <summary>
        /// The user's current location
        /// </summary>
        private Location _currentLocation = null;

        /// <summary>
        /// Location of where the compass is heading
        /// </summary>
        private readonly Location _targetLocation = new Location();

        /// <summary>
        /// True if location has been found
        /// </summary>
        private bool _isLocationFound = false;
        
        /// <summary>
        /// True when the app should get the current location
        /// </summary>
        private bool _getLocation;

        /// <summary>
        /// Page constructor
        /// </summary>
        public CompassPage()
        {
            InitializeComponent();

            // When the reading of the compass has changed, run Compass_ReadingChange
            Compass.ReadingChanged += Compass_ReadingChanged;
        }

        /// <summary>
        /// Runs on page appearance
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // Check if the application has permission to use location
            PermissionStatus hasLocationPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (hasLocationPermission != PermissionStatus.Granted)
            {
                await DisplayAlert(AppResources.error, AppResources.errorCompassNoPermission, AppResources.ok);
            } else
            {
                // Get the set endpoint coordinates
                _targetLocation.Latitude = Preferences.Get("targetLatitude", 0f);
                _targetLocation.Longitude = Preferences.Get("targetLongitude", 0f);

                // Start the compass
                try
                {
                    if (!Compass.IsMonitoring)
                    {
                        Compass.Start(SensorSpeed.UI, applyLowPassFilter: true);
                    }
                }
                catch (FeatureNotSupportedException) 
                {
                    // Device doesn't support compass
                    await DisplayAlert(AppResources.error, AppResources.errorCompassNoSupport, AppResources.ok);
                }
                catch 
                {
                    // Some other exception while starting compass
                    await DisplayAlert(AppResources.error, AppResources.errorDefault, AppResources.ok);
                }

                // Constantly get the users location, until leaving the page
                _getLocation = true;
                while (_getLocation)
                {
                    try
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Best);
                        _currentLocation = await Geolocation.GetLocationAsync(request);

                        errorLabel.IsVisible = false;
                        _isLocationFound = true;
                    }
                    catch (FeatureNotEnabledException) 
                    {
                        // GPS is not enabled
                        await DisplayAlert(AppResources.error, AppResources.errorCompassLocationOff, AppResources.ok);
                        break;
                    }
                    catch 
                    {
                        // Some other exception while getting GPS-location
                        errorLabel.IsVisible = true;
                        _isLocationFound = false;
                    }
                }
            }
        }

        /// <summary>
        /// Called on page dissapearance
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Stop getting the current location of the user
            _getLocation = false;

            // Stop the compass
            try
            {
                if (Compass.IsMonitoring)
                {
                    Compass.Stop();
                }
            }
            catch
            {
                DisplayAlert(AppResources.error, AppResources.errorDefault, AppResources.ok);
            }
        }

        /// <summary>
        /// Called when compass reading changes
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            // Get the deviation compared to north
            CompassData data = e.Reading;
            double north = data.HeadingMagneticNorth;

            // Check if the user's location was succesfully collected
            if (_isLocationFound)
            {
                // Calculate the angle the compass should head towards
                double dy = _targetLocation.Latitude - _currentLocation.Latitude;
                double dx = Math.Cos(Math.PI / 180 * _currentLocation.Latitude) * (_targetLocation.Longitude - _currentLocation.Longitude);
                double angle = Math.Atan2(dy, dx) * (180 / Math.PI);

                angle = 90 - angle;

                if (angle < 0)
                {
                    angle += 360;
                }

                // Calculate distance between user's location and endpoint
                double distance = Location.CalculateDistance(_currentLocation, _targetLocation, DistanceUnits.Kilometers) * 1000;

                // Return results
                distanceLabel.Text = string.Format("{0:f1} " + AppResources.meter, distance);

                await compassImage.RotateTo(angle - north);
            }
            else 
            {
                // Error while finding the user's location
                distanceLabel.Text = "?? " + AppResources.meter;

                await compassImage.RotateTo(0);
            }
        }

        /// <summary>
        /// Runs when the disclaimer toolbar item is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void DisclaimerToolbarItem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert(AppResources.compassPageDisclaimer, AppResources.compassPageDisclaimerText, AppResources.ok);
        }

        /// <summary>
        /// Runs when the set target toolbar item is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void SetTargetToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CompassTargetPage());
        }

        private void OpenInMapsButton_Clicked(object sender, EventArgs e)
        {
            Map.OpenAsync(_targetLocation);
        }
    }
}