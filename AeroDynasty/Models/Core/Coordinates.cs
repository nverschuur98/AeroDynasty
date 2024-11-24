using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models.Core
{
    public class Coordinates
    {
        public double Latitude;
        public double Longitude;

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public static class GeoUtilities
    {
        private const double _earthRadius = 6371.0; //in km

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Degrees value.</param>
        /// <returns>Radians value.</returns>
        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        public static double CalculateDistance(Coordinates origin, Coordinates destination)
        {
            double lonOrgRad = DegreesToRadians(origin.Longitude);
            double latOrgRad = DegreesToRadians(origin.Latitude);
            double lonDesRad = DegreesToRadians(destination.Longitude);
            double latDesRad = DegreesToRadians(destination.Latitude);

            double deltaLon = lonDesRad - lonOrgRad;
            double deltaLat = latDesRad - latOrgRad;

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(latOrgRad) * Math.Cos(latDesRad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return _earthRadius * c;
        }
    }
}
