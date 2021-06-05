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

        private void convertButton_Clicked(object sender, EventArgs e)
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

                // Convert DD to DDM
                int northDegrees = (int)Math.Floor(north);
                float northMinutes = (north - (float)Math.Floor(north)) * 60;

                int eastDegrees = (int)Math.Floor(east);
                float eastMinutes = (east - (float)Math.Floor(east)) * 60;

                // Return results
                answerLabel.Text = String.Format("N{0}° {1} E{2}° {3}", northDegrees, northMinutes, eastDegrees, eastMinutes);
            }
        }
    }
}