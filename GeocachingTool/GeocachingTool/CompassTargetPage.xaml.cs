using GeocachingTool.Model;
using GeocachingTool.Resources;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompassTargetPage : ContentPage
    {
        /// <summary>
        /// Page constructor
        /// </summary>
        public CompassTargetPage()
        {
            InitializeComponent();

            // Set default values for pickers
            latitudePicker.SelectedIndex = 0;
            longitudePicker.SelectedIndex = 0;
        }

        /// <summary>
        /// Runs on page appearance
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Get coordinates if already set
            DecimalDegreesCoordinates currentTarget = new DecimalDegreesCoordinates()
            {
                Latitude = Preferences.Get("targetLatitude", 0f),
                Longitude = Preferences.Get("targetLongitude", 0f)
            };

            // Convert DD to DDM
            DegreesDecimalMinutesCoordinates currentTargetInDegreesDecimalMinutes = currentTarget.ToDegreesDecimalMinutesCoordinates();

            // Set values
            latitudeCoordinateEntry.Text = string.Format("{0}", currentTargetInDegreesDecimalMinutes.LatitudeDegrees);
            latitudeMinuteEntry.Text = string.Format("{0:f3}", currentTargetInDegreesDecimalMinutes.LatitudeMinutes);
            latitudePicker.SelectedIndex = currentTargetInDegreesDecimalMinutes.IsNorth ? 0 : 1;

            longitudeCoordinateEntry.Text = string.Format("{0}", currentTargetInDegreesDecimalMinutes.LongitudeDegrees);
            longitudeMinuteEntry.Text = string.Format("{0:f3}", currentTargetInDegreesDecimalMinutes.LongitudeMinutes);
            longitudePicker.SelectedIndex = currentTargetInDegreesDecimalMinutes.IsEast ? 0 : 1;
        }

        /// <summary>
        /// Runs when send button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void SetButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            string latitudeDegreesEntry = latitudeCoordinateEntry.Text;
            string latitudeMinutesEntry = latitudeMinuteEntry.Text;
            string longitudeDegreesEntry = longitudeCoordinateEntry.Text;
            string longitudeMinutesEntry = longitudeMinuteEntry.Text;

            // Check if filled in
            if (string.IsNullOrEmpty(latitudeDegreesEntry) || string.IsNullOrEmpty(latitudeMinutesEntry) || string.IsNullOrEmpty(longitudeDegreesEntry) || string.IsNullOrEmpty(longitudeMinutesEntry))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            }
            else
            {
                // Create DegreesDecimalMinutesCoordinates object for storing the coordinates
                DegreesDecimalMinutesCoordinates degreesDecimalMinutesCoordinates = new DegreesDecimalMinutesCoordinates
                {
                    LatitudeDegrees = float.Parse(latitudeDegreesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    LatitudeMinutes = float.Parse(latitudeMinutesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    IsNorth = !Convert.ToBoolean(latitudePicker.SelectedIndex),
                    LongitudeDegrees = float.Parse(longitudeDegreesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    LongitudeMinutes = float.Parse(longitudeMinutesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    IsEast = !Convert.ToBoolean(longitudePicker.SelectedIndex)
                };

                // Convert to decimal degrees
                DecimalDegreesCoordinates decimalDegreesCoordinates = degreesDecimalMinutesCoordinates.ToDecimalDegreesCoordinates();

                // Set as target
                decimalDegreesCoordinates.SetAsCompassTarget();

                Navigation.PopModalAsync();
            }
        }
    }
}