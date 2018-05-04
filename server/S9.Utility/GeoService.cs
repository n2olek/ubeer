using System;
using System.Collections.Generic;
using Newtonsoft.Json;
//using System.Linq;
//using System.Text;

namespace S9.Utility
{
    public static class GeoService
    {
        /// <summary>
        /// Calculate distance between 2 geo points
        /// </summary>
        /// <param name="lat1">lat of ponit1</param>
        /// <param name="lng1">lng of point1</param>
        /// <param name="lat2">lat of point1</param>
        /// <param name="lng2">lng of point2</param>
        /// <returns>distance between 2 points</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) +
                          Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(lng1 - lng2));

            // Convert to degree
            dist = rad2deg(Math.Acos(dist));
            if(Double.IsNaN(dist))
                return 0;

            // Convert to (minute and to) mile
            dist = dist * 60 * 1.1515;

            // Convert to KM
            dist = dist * 1.609344;

            // Convert to mile
            //dist = dist * 0.8684;

            return dist;
        }

        /// <summary>
        /// check if the lat, lng in the area given leftop, right bottom
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="leftTopLat"></param>
        /// <param name="leftTopLng"></param>
        /// <param name="rightBottomLat"></param>
        /// <param name="rightBottomLng"></param>
        /// <returns>weather the lat,lng in the area</returns>

        public static bool IsInArea(double lat, double lng, double leftTopLat, double leftTopLng, double rightBottomLat, double rightBottomLng)
        {
            if(
                (rightBottomLat <= lat && leftTopLat >= lat) && 
                (leftTopLng <= lng && rightBottomLng >= lng)
               )
                return true;

            return false;
        }

        /// <summary>
        /// Convert value from degree to radian
        /// </summary>
        /// <param name="deg"></param>
        /// <returns>value in radian</returns>
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        /// <summary>
        /// Convert value from radian to degree
        /// </summary>
        /// <param name="rad"></param>
        /// <returns>value in degree</returns>
        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }


        
        /// <summary>
        /// Check if the location is in boundary
        /// </summary>
        /// <param name="lat">coordinate to check</param>
        /// <param name="lng">coordinate to check</param>
        /// <param name="ringRouteJSON">JSON of the route</param>
        /// <returns></returns>
        public static bool IsLocationInRing(double lat, double lng, string ringRouteJSON)
        {
            // read from file
            ListGeoLocation lst = JsonConvert.DeserializeObject<ListGeoLocation>(ringRouteJSON);

            // expected true
            return GeoService.IsInRing(new GeoLocation(lat, lng), lst);
        }



        /// <summary>
        /// Convert from boundary JSON to list of location
        /// </summary>
        /// <param name="lat">coordinate to check</param>
        /// <param name="lng">coordinate to check</param>
        /// <param name="boundaryJSON">JSON of the route</param>
        /// <returns></returns>
        public static ListGeoLocation GetRing(string boundaryJSON)
        {
            ListGeoLocation lst = JsonConvert.DeserializeObject<ListGeoLocation>(boundaryJSON);
            return lst;
        }

        
        /// <summary>
        /// Is in Ring
        /// </summary>
        /// <param name="locToCheck"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        private static bool IsInRing(GeoLocation locToCheck, ListGeoLocation locations)
        {
            int i, j;
            bool result = false;

            for (i = 0, j = locations.data.Count - 1; i < locations.data.Count; j = i++)
            {
                if ((((locations.data[i].lat <= locToCheck.lat) && (locToCheck.lat < locations.data[j].lat))
                        || ((locations.data[j].lat <= locToCheck.lat) && (locToCheck.lat < locations.data[i].lat)))
                        && (locToCheck.lng < (locations.data[j].lng - locations.data[i].lng) * (locToCheck.lat - locations.data[i].lat)
                            / (locations.data[j].lat - locations.data[i].lat) + locations.data[i].lng))

                    result = !result;
            }

            return result;
        }
    }

    public class GeoLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }

        public GeoLocation(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }

    }

    public class ListGeoLocation
    {
        public List<GeoLocation> data { get; set; }
    }
}
