using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.MapCalculator
{
    public class MapPoint
    {
        public double Latitude = 0.0;
        public double Longitude = 0.0;
        public QuandrantResult Result = null;

        public MapPoint()
        {
        }

        public MapPoint(double longitude, double latitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
