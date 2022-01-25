using GeocachingTool.Handler;
using GeocachingTool.Model;
using GeocachingTool.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoordinateDDMtoDDPage : ContentPage
    {
        public CoordinateDDMtoDDPage()
        {
            InitializeComponent();

            // Set default values for pickers
            latitudePicker.SelectedIndex = 0;
            longitudePicker.SelectedIndex = 0;
        }

        private void ConvertButton_Clicked(object sender, EventArgs e)
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
            } else
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

                // Return results
                answerLabel.Text = string.Format("{0:f5}; {1:f5}", decimalDegreesCoordinates.Latitude, decimalDegreesCoordinates.Longitude);

                // Review handling
                ReviewHandler reviewHandler = new ReviewHandler();
                reviewHandler.AskReviewAfterUsage();
            }
        }
    }
}