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
    public partial class CoordinateDDtoDDMPage : ContentPage
    {
        public CoordinateDDtoDDMPage()
        {
            InitializeComponent();
        }

        private void ConvertButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            string northInput = northEntry.Text;
            string eastInput = eastEntry.Text;

            // Check if filled in
            if (String.IsNullOrEmpty(northInput) || String.IsNullOrEmpty(eastInput))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            } else
            {
                // Convert to floats
                float north = float.Parse(northInput.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float east = float.Parse(eastInput.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);

                // Check if north/south or east/west
                string northLabel = "N";
                string eastLabel = "E";

                if (north < 0)
                {
                    north *= -1;
                    northLabel = "S";
                }

                if (east < 0)
                {
                    east *= -1;
                    eastLabel = "W";
                }

                // Convert DD to DDM
                int northDegrees = (int)Math.Floor(north);
                float northMinutes = (north - (float)Math.Floor(north)) * 60;

                int eastDegrees = (int)Math.Floor(east);
                float eastMinutes = (east - (float)Math.Floor(east)) * 60;

                // Return results
                answerLabel.Text = String.Format("{0}{1}° {2} {3}{4}° {5}", northLabel, northDegrees, northMinutes, eastLabel, eastDegrees, eastMinutes);
            }
        }
    }
}