using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GeocachingTool.Model
{
    /// <summary>
    /// Coordinates in decimal degrees format
    /// </summary>
    class DecimalDegreesCoordinates
    {
        /// <summary>
        /// Latitude (north/south)
        /// </summary>
        public float Latitude { get; set; } = 0;

        /// <summary>
        /// Longitude (east/west)
        /// </summary>
        public float Longitude { get; set; } = 0;

        /// <summary>
        /// Convert coordinates to degrees and decimal minutes
        /// </summary>
        /// <returns>The coordinates in degrees and decimal minutes format</returns>
        public DegreesDecimalMinutesCoordinates ToDegreesDecimalMinutesCoordinates()
        {
            DegreesDecimalMinutesCoordinates degreesDecimalMinutesCoordinates = new DegreesDecimalMinutesCoordinates
            {
                // Convert DD to DDM
                LatitudeDegrees = (int)Math.Floor(Math.Abs(Latitude)),
                LatitudeMinutes = (Math.Abs(Latitude) - (float)Math.Floor(Math.Abs(Latitude))) * 60,

                LongitudeDegrees = (int)Math.Floor(Math.Abs(Longitude)),
                LongitudeMinutes = (Math.Abs(Longitude) - (float)Math.Floor(Math.Abs(Longitude))) * 60
            };

            // Check for south and west
            if (Latitude < 0)
            {
                degreesDecimalMinutesCoordinates.IsNorth = false;
            }

            if (Longitude < 0)
            {
                degreesDecimalMinutesCoordinates.IsEast = false;
            }

            // Return coordinates
            return degreesDecimalMinutesCoordinates;
        }

        /// <summary>
        /// Calculate a projection from the coordinates
        /// </summary>
        /// <param name="bearing">The bearing in degrees</param>
        /// <param name="distance">The distance in kilometers</param>
        /// <returns>The coordinates of the projection</returns>
        public DecimalDegreesCoordinates CalculateProjection(float bearing, float distance)
        {
            double earthRadius = 6371.01;
            double distanceRatio = distance / earthRadius;
            double distanceRatioSine = Math.Sin(distanceRatio);
            double distanceRatioCosine = Math.Cos(distanceRatio);

            double startLatitutdeRadians = DegreesToRadians(Latitude);
            double startLongitudeRadians = DegreesToRadians(Longitude);

            double startLatitudeCos = Math.Cos(startLatitutdeRadians);
            double startLatitudeSin = Math.Sin(startLatitutdeRadians);

            double endLatitutdeRads = Math.Asin((startLatitudeSin * distanceRatioCosine) + (startLatitudeCos * distanceRatioSine * Math.Cos(DegreesToRadians(bearing))));

            double endLongitudeRadians = startLongitudeRadians + Math.Atan2(Math.Sin(DegreesToRadians(bearing)) * distanceRatioSine * startLatitudeCos, distanceRatioCosine - startLatitudeSin * Math.Sin(endLatitutdeRads));

            return new DecimalDegreesCoordinates { Latitude = (float)RadiansToDegrees(endLatitutdeRads), Longitude = (float)RadiansToDegrees(endLongitudeRadians) };
        }

        /// <summary>
        /// Sets the coordinates as the new target for the compass
        /// </summary>
        public void SetAsCompassTarget()
        {
            Preferences.Set("targetLatitude", Latitude);
            Preferences.Set("targetLongitude", Longitude);
        }

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="degrees">The degrees to convert</param>
        /// <returns>The radians</returns>
        private static double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }

        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="radians">The radians to convert</param>
        /// <returns>The degrees</returns>
        private static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }
    }
}
