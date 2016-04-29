using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LatLongFinder
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter Latitude: ");
            var startLatitude = ConvertToRadians(Double.Parse(Console.ReadLine())); //#Current lat point converted to radians
            Console.WriteLine("Enter Longitude: ");
            var startLongitude = ConvertToRadians(Double.Parse(Console.ReadLine())); //#Current long point converted to radians
            Console.WriteLine("Enter Bearing: ");
            var bearing = ConvertToRadians(Double.Parse(Console.ReadLine()));//Bearing converted to decimal
            Console.WriteLine("Enter Distance: ");

            var earthRadius = WGS84EarthRadius(startLatitude); //#Radius of the Earth in kilometers
            var range = Double.Parse(Console.ReadLine()) * 0.0003048; //#Distance in feet converted to km

            var destinationLatitude = Math.Asin(Math.Sin(startLatitude) * Math.Cos(range / earthRadius) +
                 Math.Cos(startLatitude) * Math.Sin(range / earthRadius) * Math.Cos(bearing));

            var destinationLongitude = startLongitude + Math.Atan2(Math.Sin(bearing) * Math.Sin(range / earthRadius) * Math.Cos(startLatitude),
                         Math.Cos(range / earthRadius) - Math.Sin(startLatitude) * Math.Sin(destinationLatitude));

            destinationLatitude = RadianToDegree(destinationLatitude);
            destinationLongitude = RadianToDegree(destinationLongitude);

            Console.WriteLine("Lat: {0}", destinationLatitude);
            Console.WriteLine("Lon: {0}", destinationLongitude);
            Console.ReadLine();
        }

        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        // Earth radius at a given latitude, according to the WGS-84 ellipsoid [m]
        private static double WGS84EarthRadius(double latitude)
        {
            /*http://en.wikipedia.org/wiki/Earth_radius
            *  WGS84_a =  Equatorial radius (6,378.1370 km)
            *  WGS84_b =  Polar radius (6,356.7523 km)
            */
            var WGS84_a = 6378.1370;
            var WGS84_b = 6356.7523;

            var An = Math.Pow(WGS84_a, 2) * Math.Cos(latitude);
            var Bn = Math.Pow(WGS84_b, 2) * Math.Sin(latitude);
            var Ad = WGS84_a * Math.Cos(latitude);
            var Bd = WGS84_b * Math.Sin(latitude);
            return Math.Sqrt((An * An + Bn * Bn) / (Ad * Ad + Bd * Bd));
        }
    }
}
