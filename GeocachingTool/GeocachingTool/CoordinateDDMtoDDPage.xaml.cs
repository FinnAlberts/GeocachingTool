﻿using GeocachingTool.Resources;
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
    public partial class CoordinateDDMtoDDPage : ContentPage
    {
        public CoordinateDDMtoDDPage()
        {
            InitializeComponent();

            // Set default values for pickers
            northPicker.SelectedIndex = 0;
            eastPicker.SelectedIndex = 0;
        }

        private void convertButton_Clicked(object sender, EventArgs e)
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
            } else
            {
                // Convert to floats
                float northDegrees = float.Parse(northDegreesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float northMinutes = float.Parse(northMinutesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float eastDegrees = float.Parse(eastDegreesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float eastMinutes = float.Parse(eastMinutesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);

                // Convert DDM to DD
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

                // Return results
                answerLabel.Text = String.Format("{0} N {1} E", north, east);
            }
        }
    }
}