using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.MapCalculator
{
    public class QuadrantArray
    {
        public MapPoint[,] Points = null;
        public Quadrant[,] Quadrants = null;
        public double[] Longitudes = null;
        public double[] Latitudes = null;

        public QuadrantArray(Quadrant map, int longitudeParts, int latitudeParts)
        {
            double longitudeMin = map.LongitudeStart;
            double longitudeMax = map.LongitudeEnd;
            double latitudeMin = map.LatitudeStart;
            double latitudeMax = map.LatitudeEnd;

            if (longitudeParts < 2) longitudeParts = 2;
            if (latitudeParts < 2) latitudeParts = 2;

            double longitudeDiff = (longitudeMax - longitudeMin) / longitudeParts;
            double latitudeDiff = (latitudeMax - latitudeMin) / latitudeParts;

            Points = new MapPoint[longitudeParts + 1, latitudeParts + 1];
            Quadrants = new Quadrant[longitudeParts, latitudeParts];
            Longitudes = new double[longitudeParts + 1];
            Latitudes = new double[latitudeParts + 1];

            for (int la = 0; la <= latitudeParts; la++)
            {
                Latitudes[la] = latitudeMin + latitudeDiff * la;
            }

            for (int lo = 0; lo <= longitudeParts; lo++)
            {
                Longitudes[lo] = longitudeMin + longitudeDiff * lo;
                for (int la = 0; la <= latitudeParts; la++)
                {
                    Points[lo, la] = new MapPoint();
                    Points[lo, la].Longitude = Longitudes[lo];
                    Points[lo, la].Latitude = Latitudes[la];
                }
            }

            Points[0, 0] = map.WS;
            Points[0, latitudeParts] = map.WN;
            Points[longitudeParts, 0] = map.ES;
            Points[longitudeParts, latitudeParts] = map.EN;

            for (int lo = 0; lo < longitudeParts; lo++)
            {
                for (int la = 0; la < latitudeParts; la++)
                {
                    Quadrants[lo, la] = new Quadrant(Points[lo, la], Points[lo, la + 1], Points[lo + 1, la], Points[lo + 1, la + 1],
                        map.pixelsX / longitudeParts, map.pixelsY / latitudeParts);
                }
            }
        }
    }

}
