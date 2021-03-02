using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompassPage : ContentPage
    {
        private Location startLocation = null;
        private Location endLocation = new Location();
        private bool locationFound = false;
        private bool getLocation;


        public CompassPage()
        {
            InitializeComponent();

            // When the reading of the compass has changed, run Compass_ReadingChange
            Compass.ReadingChanged += Compass_ReadingChanged;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // Check if the application has permission to use location
            var hasLocationPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (hasLocationPermission != PermissionStatus.Granted) // No permission to use location
            {
                await DisplayAlert("Fout", "Voor het gebruiken van het kompas is je locatie nodig.", "Oke");
            } else
            {
                // Get the set endpoint coordinates
                endLocation.Latitude = Preferences.Get("targetLatitude", 0f);
                endLocation.Longitude = Preferences.Get("targetLongitude", 0f);

                // Start the compass
                try
                {
                    if (!Compass.IsMonitoring)
                    {
                        Compass.Start(SensorSpeed.UI, applyLowPassFilter: true);
                    }
                }
                catch (FeatureNotSupportedException) // Device doesn't support compass
                {
                    await DisplayAlert("Fout", "Je apparaat biedt helaas geen ondersteuning voor een kompas.", "Oke");
                }
                catch // Some other exception while starting compass
                {
                    await DisplayAlert("Fout", "Er is iets misgegaan. Probeer het opnieuw.", "Oke");
                }

                // Constantly get the users location, until leaving the page
                getLocation = true;
                while (getLocation)
                {
                    try
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Best);
                        startLocation = await Geolocation.GetLocationAsync(request);

                        errorLabel.IsVisible = false;
                        locationFound = true;
                    }
                    catch (FeatureNotEnabledException) // GPS is not enabled
                    {
                        await DisplayAlert("Fout", "De locatie op je apparaat is niet ingeschakeld.", "Oke");
                        break;
                    }
                    catch // Some other exception while getting GPS-location
                    {
                        errorLabel.IsVisible = true;
                        locationFound = false;
                    }
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Stop getting the current location of the user
            getLocation = false;

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
                DisplayAlert("Fout", "Er is iets misgegaan. Probeer het opnieuw.", "Oke");
            }
        }

        async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            // Get the deviation compared to north
            var data = e.Reading;
            double north = data.HeadingMagneticNorth;

            // Check if the user's location was succesfully collected
            if (locationFound)
            {
                // Calculate the angle the compass should head towards
                double dy = endLocation.Latitude - startLocation.Latitude;
                double dx = Math.Cos(Math.PI / 180 * startLocation.Latitude) * (endLocation.Longitude - startLocation.Longitude);
                double angle = Math.Atan2(dy, dx) * (180 / Math.PI);

                angle = 360 - (angle + 180) - 90;

                if (angle < 0)
                {
                    angle += 360;
                }

                // Calculate distance between user's location and endpoint
                double distance = Location.CalculateDistance(startLocation, endLocation, DistanceUnits.Kilometers) * 1000;

                // Return results
                distanceLabel.Text = String.Format("{0:f1} meter", distance);

                await compassImage.RotateTo(angle - north);
            }
            else // Error while finding the user's location
            {
                distanceLabel.Text = "?? meter";

                await compassImage.RotateTo(0);
            }
            
        }

        private void disclaimerToolbarItem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Disclaimer", "Hou er rekening mee dat de accuraatheid van het kompas en de afstand sterk afhangt van je apparaat. Je locatie wordt ongeveer iedere 15 seconden bijgewerkt en is tot op ongeveer 10 meter nauwkeurig. Ook dit hangt af van je apparaat. Hou hier rekening mee tijdens het cachen.", "Oke");
        }

        private void setTargetToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CompassTargetPage());
        }
    }
}