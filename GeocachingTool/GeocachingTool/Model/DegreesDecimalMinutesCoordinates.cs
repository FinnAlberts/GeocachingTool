using System;
using System.Collections.Generic;
using System.Text;

namespace GeocachingTool.Model
{
    /// <summary>
    /// Coordinates in degrees and decimal minutes format
    /// </summary>
    class DegreesDecimalMinutesCoordinates
    {
        /// <summary>
        /// Degrees latitude (north/south)
        /// </summary>
        public float LatitudeDegrees { get; set; } = 0;

        /// <summary>
        /// Minutes latitude (north/south)
        /// </summary>
        public float LatitudeMinutes { get; set; } = 0;

        /// <summary>
        /// Wether the latitude is north or south (true for north)
        /// </summary>
        public bool IsNorth { get; set; } = true;

        /// <summary>
        /// Degrees longitude (east/west)
        /// </summary>
        public float LongitudeDegrees { get; set; } = 0;

        /// <summary>
        /// Minutes longitude (east/west)
        /// </summary>
        public float LongitudeMinutes { get; set; } = 0;

        /// <summary>
        /// Wether the longitude is east or west (true for east)
        /// </summary>
        public bool IsEast { get; set; } = true;

        /// <summary>
        /// Convert the coordinates to decimal degrees
        /// </summary>
        /// <returns>The coordinates in decimal degrees format</returns>
        public DecimalDegreesCoordinates ToDecimalDegreesCoordinates()
        {
            DecimalDegreesCoordinates decimalDegreesCoordinates = new DecimalDegreesCoordinates
            {
                // Convert from DDM to DD
                Latitude = Math.Abs(LatitudeDegrees) + LatitudeMinutes / 60,
                Longitude = Math.Abs(LongitudeDegrees) + LongitudeMinutes / 60
            };

            // Check for south and west
            if (!IsNorth)
            {
                decimalDegreesCoordinates.Latitude *= -1;
            }

            if (!IsEast)
            {
                decimalDegreesCoordinates.Longitude *= -1;
            }

            // Return coordinates
            return decimalDegreesCoordinates;
        }

        /// <summary>
        /// Retyrbs the label for latitude (N for north and S for south)
        /// </summary>
        /// <returns>The label</returns>
        public char GetLatitudeLabel()
        {
            if (IsNorth)
            {
                return 'N';
            }
            else
            {
                return 'S';
            }
        }

        /// <summary>
        /// Returns the label for the longitude (E for east and W for west)
        /// </summary>
        /// <returns>The label</returns>
        public char GetLongitudeLabel()
        {
            if (IsEast)
            {
                return 'E';
            } 
            else
            {
                return 'W';
            }
        }
    }
}
