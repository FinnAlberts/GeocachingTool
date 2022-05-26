using GeocachingTool.Handler;
using GeocachingTool.Model;
using GeocachingTool.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoordinateDDtoDDMPage : ContentPage
    {
        /// <summary>
        /// The inputed coordinates
        /// </summary>
        private DecimalDegreesCoordinates _decimalDegreesCoordinates;

        /// <summary>
        /// Page constructor
        /// </summary>
        public CoordinateDDtoDDMPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs when the convert button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void ConvertButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            string latitudeInput = latitudeEntry.Text;
            string longitudeInput = longitudeEntry.Text;

            // Check if filled in
            if (string.IsNullOrEmpty(latitudeInput) || string.IsNullOrEmpty(longitudeInput))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            } else
            {
                // Create DecimalDegreesCoordinates object for storing the coordinates
                _decimalDegreesCoordinates = new DecimalDegreesCoordinates
                {
                    Latitude = float.Parse(latitudeInput.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    Longitude = float.Parse(longitudeInput.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture)
                };

                // Convert DD to DDM
                DegreesDecimalMinutesCoordinates degreesDecimalMinutesCoordinates = _decimalDegreesCoordinates.ToDegreesDecimalMinutesCoordinates();

                // Return results
                answerLabel.Text = string.Format("{0}{1}° {2:f3} {3}{4}° {5:f3}", degreesDecimalMinutesCoordinates.GetLatitudeLabel(), degreesDecimalMinutesCoordinates.LatitudeDegrees, degreesDecimalMinutesCoordinates.LatitudeMinutes, degreesDecimalMinutesCoordinates.GetLongitudeLabel(), degreesDecimalMinutesCoordinates.LongitudeDegrees, degreesDecimalMinutesCoordinates.LongitudeMinutes);

                // Show set as compass target button
                setAsCompassTargetButton.IsVisible = true;

                // Review handling
                ReviewHandler.AskReviewAfterUsage();
            }
        }

        /// <summary>
        /// Runs when set as compass target button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void SetAsCompassTargetButton_Clicked(object sender, EventArgs e)
        {
            _decimalDegreesCoordinates.SetAsCompassTarget();
            DisplayAlert(AppResources.succes, AppResources.succesfullySetCompassTarget, AppResources.ok);
        }
    }
}