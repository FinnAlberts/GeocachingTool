using GeocachingTool.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculateProjectionCoordinatePage : ContentPage
    {
        public CalculateProjectionCoordinatePage()
        {
            InitializeComponent();

            // Set default values for pickers
            northPicker.SelectedIndex = 0;
            eastPicker.SelectedIndex = 0;
        }

        private void calculateButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            string northDegreesEntry = northCoordinateEntry.Text;
            string northMinutesEntry = northMinuteEntry.Text;
            string eastDegreesEntry = eastCoordinateEntry.Text;
            string eastMinutesEntry = eastMinuteEntry.Text;
            string bearingEntry = angleEntry.Text;
            string meteresEntry = distanceEntry.Text;

            // Check if filled in
            if (String.IsNullOrEmpty(northDegreesEntry) || String.IsNullOrEmpty(northMinutesEntry) || String.IsNullOrEmpty(eastDegreesEntry) || String.IsNullOrEmpty(eastMinutesEntry) || String.IsNullOrEmpty(bearingEntry) || String.IsNullOrEmpty(meteresEntry))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            }
            else
            {
                // Convert to floats
                float northDegrees = float.Parse(northDegreesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float northMinutes = float.Parse(northMinutesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float eastDegrees = float.Parse(eastDegreesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float eastMinutes = float.Parse(eastMinutesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float bearing = float.Parse(bearingEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float distance = float.Parse(meteresEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture) / 1000;

                // Convert from DDM to DD
                float north = northDegrees + northMinutes / 60;
                float east = eastDegrees + eastMinutes / 60;

                // Check for south and west
                if (northPicker.SelectedIndex == 1)
                {
                    north *= -1;
                }

                if (eastPicker.SelectedIndex == 1)
                {
                    east *= -1;
                }

                // Calculate endpoint
                double earthRadius = 6371.01;
                var distRatio = distance/ earthRadius;
                var distRatioSine = Math.Sin(distRatio);
                var distRatioCosine = Math.Cos(distRatio);

                var startLatRad = DegreesToRadians(north);
                var startLonRad = DegreesToRadians(east);

                var startLatCos = Math.Cos(startLatRad);
                var startLatSin = Math.Sin(startLatRad);

                var endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(DegreesToRadians(bearing))));

                var endLonRads = startLonRad + Math.Atan2(Math.Sin(DegreesToRadians(bearing)) * distRatioSine * startLatCos, distRatioCosine - startLatSin * Math.Sin(endLatRads));

                var endNorth = RadiansToDegrees(endLatRads);
                var endEast = RadiansToDegrees(endLonRads);

                string northLabel = "N";
                string eastLabel = "E";

                if (endNorth < 0)
                {
                    endNorth *= -1;
                    northLabel = "S";
                }

                if (endEast < 0)
                {
                    endEast *= -1;
                    eastLabel = "W";
                }

                var northDegreesAnswer = (int)Math.Floor(endNorth);
                var northMinutesAnswer = (endNorth - (float)Math.Floor(endNorth)) * 60;

                var eastDegreesAnswer = (int)Math.Floor(endEast);
                var eastMinutesAnswer = (endEast - (float)Math.Floor(endEast)) * 60;

                // Return answer
                answerLabel.Text = String.Format("{0}{1}° {2:f3} {3}{4}° {5:f3}", northLabel, northDegreesAnswer, northMinutesAnswer, eastLabel, eastDegreesAnswer, eastMinutesAnswer);
            }
        }

        // Convert degrees to radians
        public static double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }

        // Convert radians to degrees
        public static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }
    }
}