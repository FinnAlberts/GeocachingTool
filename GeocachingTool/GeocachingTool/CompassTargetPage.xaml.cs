using GeocachingTool.Resources;
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
    public partial class CompassTargetPage : ContentPage
    {
        public CompassTargetPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Get set coordiantes if already set
            float north = Preferences.Get("targetLatitude", 0f);
            float east = Preferences.Get("targetLongitude", 0f);

            // Convert DD to DDM
            int northDegrees = (int)Math.Floor(north);
            float northMinutes = (north - (float)Math.Floor(north)) * 60;

            int eastDegrees = (int)Math.Floor(east);
            float eastMinutes = (east - (float)Math.Floor(east)) * 60;

            // Put set coordinates into entries
            northCoordinateEntry.Text = northDegrees.ToString();
            northMinuteEntry.Text = northMinutes.ToString();

            eastCoordinateEntry.Text = eastDegrees.ToString();
            eastMinuteEntry.Text = eastMinutes.ToString();
        }

        private void setButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            string northDegreesEntry = northCoordinateEntry.Text;
            string northMinutesEntry = northMinuteEntry.Text;
            string eastDegreesEntry = eastCoordinateEntry.Text;
            string eastMinutesEntry = eastMinuteEntry.Text;

            // Check if filled in
            if (String.IsNullOrEmpty(northDegreesEntry) || String.IsNullOrEmpty(northMinutesEntry) || String.IsNullOrEmpty(eastDegreesEntry) || String.IsNullOrEmpty(eastMinutesEntry))
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

                // Convert DDM to DD
                float north = northDegrees + northMinutes / 60;
                float east = eastDegrees + eastMinutes / 60;

                // Save coordinates
                Preferences.Set("targetLatitude", north);
                Preferences.Set("targetLongitude", east);

                Navigation.PopModalAsync();
            }
        }
    }
}