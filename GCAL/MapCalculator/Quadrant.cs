using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.MapCalculator
{
    public class Quadrant
    {
        public MapPoint WS;
        public MapPoint WN;
        public MapPoint ES;
        public MapPoint EN;

        public double pixelsX = 0;
        public double pixelsY = 0;

        public QuadrantResultState ResultState
        {
            get
            {
                if (WS.Result == null || WN.Result == null
                    || ES.Result == null || EN.Result == null)
                    return QuadrantResultState.NotAvailable;

                if (WS.Result.ResultId == WN.Result.ResultId
                    && WN.Result.ResultId == ES.Result.ResultId
                    && ES.Result.ResultId == EN.Result.ResultId)
                    return QuadrantResultState.Consistent;

                return EligibleForDecomposition
                    ? QuadrantResultState.Decomposable
                    : QuadrantResultState.Inconsistent;
            }
        }

        public QuandrantResult Result
        {
            get
            {
                if (WS.Result == null || WN.Result == null
                    || ES.Result == null || EN.Result == null)
                    return null;

                if (WS.Result.ResultId == WN.Result.ResultId
                    && WN.Result.ResultId == ES.Result.ResultId
                    && ES.Result.ResultId == EN.Result.ResultId)
                    return WS.Result;

                return null;
            }
        }

        public bool EligibleForDecomposition
        {
            get
            {
                return (pixelsX > 3.0 && pixelsY > 3.0);
            }
        }

        public QuadrantArray Details = null;

        public Quadrant(GCMap map)
        {
            WS = new MapPoint(map.LongitudeStart, map.LatitudeStart);
            WN = new MapPoint(map.LongitudeStart, map.LatitudeEnd);
            ES = new MapPoint(map.LongitudeEnd, map.LatitudeStart);
            EN = new MapPoint(map.LongitudeEnd, map.LatitudeEnd);
            pixelsX = map.ImageSize.Width;
            pixelsY = map.ImageSize.Height;
        }

        public Quadrant(double longWest, double longEast, double latSouth, double latNorth, double pixelx, double pixelsy)
        {
            WS = new MapPoint(longWest, latSouth);
            WN = new MapPoint(longWest, latNorth);
            ES = new MapPoint(longEast, latSouth);
            EN = new MapPoint(longEast, latNorth);
            pixelsX = pixelx;
            pixelsY = pixelsy;
        }

        public Quadrant(MapPoint ws, MapPoint wn, MapPoint es, MapPoint en, double pixelx, double pixelsy)
        {
            WS = ws;
            WN = wn;
            ES = es;
            EN = en;
            pixelsX = pixelx;
            pixelsY = pixelsy;
        }

        public void Trim()
        {
            if (WS.Latitude < -66)
                WS.Latitude = -66;
            if (ES.Latitude < -66)
                ES.Latitude = -66;
            if (WN.Latitude > 66)
                WN.Latitude = 66;
            if (EN.Latitude > 66)
                EN.Latitude = 66;
            if (WN.Longitude < -180)
                WN.Longitude = -180;
            if (WS.Longitude < -180)
                WS.Longitude = -180;
            if (EN.Longitude > 180)
                EN.Longitude = 180;
            if (ES.Longitude > 180)
                ES.Longitude = 180;
        }

        public double LongitudeStart { get { return WS.Longitude; } }
        public double LongitudeEnd { get { return ES.Longitude; } }
        public double LatitudeStart { get { return WS.Latitude; } }
        public double LatitudeEnd { get { return WN.Latitude; } }
        public double LongitudeRange { get { return ES.Longitude - WS.Longitude; } }
        public double LatitudeRange { get { return WN.Latitude - WS.Latitude; } }
    }

}
