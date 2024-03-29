﻿using GeocachingTool.Handler;
using GeocachingTool.Model;
using GeocachingTool.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeocachingTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculateProjectionCoordinatePage : ContentPage
    {
        /// <summary>
        /// The end point of the projection
        /// </summary>
        private DecimalDegreesCoordinates _endPoint;

        /// <summary>
        /// Page constructor
        /// </summary>
        public CalculateProjectionCoordinatePage()
        {
            InitializeComponent();

            // Set default values for pickers
            latitudePicker.SelectedIndex = 0;
            longitudePicker.SelectedIndex = 0;
        }

        /// <summary>
        /// Runs when calculate button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            // Get input
            string latitudeDegreesEntry = latitudeCoordinateEntry.Text;
            string latitudeMinutesEntry = latitudeMinuteEntry.Text;
            string longitudeDegreesEntry = longitudeCoordinateEntry.Text;
            string longitudeMinutesEntry = longitudeMinuteEntry.Text;
            string bearingEntry = angleEntry.Text;
            string meteresEntry = distanceEntry.Text;

            // Check if filled in
            if (string.IsNullOrEmpty(latitudeDegreesEntry) || string.IsNullOrEmpty(latitudeMinutesEntry) || string.IsNullOrEmpty(longitudeDegreesEntry) || string.IsNullOrEmpty(longitudeMinutesEntry) || string.IsNullOrEmpty(bearingEntry) || string.IsNullOrEmpty(meteresEntry))
            {
                DisplayAlert(AppResources.error, AppResources.notAllFieldFilledIn, AppResources.ok);
            }
            else
            {
                // Set coordinates in DegreesDecimalMinutes object
                DegreesDecimalMinutesCoordinates startPointInDegreesDecimalMinutes = new DegreesDecimalMinutesCoordinates
                {
                    LatitudeDegrees = float.Parse(latitudeDegreesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    LatitudeMinutes = float.Parse(latitudeMinutesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    IsNorth = !Convert.ToBoolean(latitudePicker.SelectedIndex),
                    LongitudeDegrees = float.Parse(longitudeDegreesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    LongitudeMinutes = float.Parse(longitudeMinutesEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture),
                    IsEast = !Convert.ToBoolean(longitudePicker.SelectedIndex)
                };

                // Convert DDM to DD
                DecimalDegreesCoordinates startPoint = startPointInDegreesDecimalMinutes.ToDecimalDegreesCoordinates();

                // Get bearing and distance
                float bearing = float.Parse(bearingEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                float distance = float.Parse(meteresEntry.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture) / 1000;

                // Calculate projection
                _endPoint = startPoint.CalculateProjection(bearing, distance);

                // Convert to DegreesDecimalMinutes
                DegreesDecimalMinutesCoordinates endPointDegreesDecimalMinutes = _endPoint.ToDegreesDecimalMinutesCoordinates();

                // Return answer
                answerLabel.Text = string.Format("{0}{1}° {2:f3} {3}{4}° {5:f3}", endPointDegreesDecimalMinutes.GetLatitudeLabel(), endPointDegreesDecimalMinutes.LatitudeDegrees, endPointDegreesDecimalMinutes.LatitudeMinutes, endPointDegreesDecimalMinutes.GetLongitudeLabel(), endPointDegreesDecimalMinutes.LongitudeDegrees, endPointDegreesDecimalMinutes.LongitudeMinutes);

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
            _endPoint.SetAsCompassTarget();
            DisplayAlert(AppResources.succes, AppResources.succesfullySetCompassTarget, AppResources.ok);
        }
    }
}