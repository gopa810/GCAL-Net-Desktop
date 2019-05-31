using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.VSOP87
{
    public class GCVSOPAstronomy
    {
        private static Dictionary<int, GCAstronomyCoordPage> pages = new Dictionary<int, GCAstronomyCoordPage>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bodyId">0 - sun, 1-moon, 2-mars, 3-mercury, 4-jupiter, 5-venus, 6-saturn, 7-rahu, 8-ketu, 9-ascendant</param>
        /// <param name="julianDate"></param>
        /// <param name="L">Longitude in degrees</param>
        /// <param name="B">Latitude in degrees</param>
        /// <param name="R">Radius in AU</param>
        public static void GetGeocentricCoordinates(int bodyId, double julianDate, out double L, out double B, out double R)
        {
            // for sun and moon calculate always fresh values
            if (bodyId == 0)
            {
                GCEarth.Calculate(julianDate, out L, out B, out R);
                L = GCMath.putIn360(L + 180);
                B = -B;
                return;
            }
            else if (bodyId == 1)
            {
                GCMoonData.Calculate(julianDate, out L, out B, out R);
                return;
            }
            else
            {
                GCAstronomyCoordPage page = null;
                int b = Convert.ToInt32(Math.Floor(julianDate));
                int a = b / GCAstronomyCoordPage.PAGE_SIZE;
                int Base = a * GCAstronomyCoordPage.PAGE_SIZE;
                int Index = b - Base + GCAstronomyCoordPage.PAGE_HEAD;
                double fraction = julianDate - b;

                if (pages.ContainsKey(a))
                {
                    page = pages[a];
                }
                else
                {
                    page = new GCAstronomyCoordPage(Base);
                    pages.Add(a, page);
                }

                if (!page.dataInit[bodyId])
                {
                    page.InitializePlanet(bodyId);
                }

                L = GCMath.putIn360(Interpolate(page.data[bodyId, Index - 2, 0], 
                    page.data[bodyId, Index - 1, 0], page.data[bodyId, Index, 0],
                    page.data[bodyId, Index + 1, 0], page.data[bodyId, Index + 2, 0], fraction));
                B = Interpolate(page.data[bodyId, Index - 2, 1], 
                    page.data[bodyId, Index - 1, 1], page.data[bodyId, Index, 1],
                    page.data[bodyId, Index + 1, 1], page.data[bodyId, Index + 2, 1], fraction);
                R = Interpolate(page.data[bodyId, Index - 2, 2], 
                    page.data[bodyId, Index - 1, 2], page.data[bodyId, Index, 2],
                    page.data[bodyId, Index + 1, 2], page.data[bodyId, Index + 2, 2], fraction);
            }
        }

        public static double GetPlanetLongitude(int bodyId, double julianDate)
        {
            double l, b, r;
            GetGeocentricCoordinates(bodyId, julianDate, out l, out b, out r);
            return l;
        }

        public static double GetRawLongitude(int bodyId, int julianDay)
        {
            double L, B, R;
            // for sun and moon calculate always fresh values
            if (bodyId == 0)
            {
                GCEarth.Calculate(julianDay, out L, out B, out R);
                return GCMath.putIn360(L + 180);
            }
            else if (bodyId == 1)
            {
                GCMoonData.Calculate(julianDay, out L, out B, out R);
                return L;
            }
            else
            {
                GCAstronomyCoordPage page = null;
                int b = julianDay;
                int a = b / GCAstronomyCoordPage.PAGE_SIZE;
                int Base = a * GCAstronomyCoordPage.PAGE_SIZE;
                int Index = b - Base + GCAstronomyCoordPage.PAGE_HEAD;

                if (pages.ContainsKey(a))
                {
                    page = pages[a];
                }
                else
                {
                    page = new GCAstronomyCoordPage(Base);
                    pages.Add(a, page);
                }

                if (!page.dataInit[bodyId])
                {
                    page.InitializePlanet(bodyId);
                }

                return page.data[bodyId, Index, 0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y0"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="y3"></param>
        /// <param name="y4"></param>
        /// <param name="fraction">fraction added x2, to calculate its y value</param>
        /// <returns></returns>
        public static double Interpolate(double y1, double y2, double y3, double y4, double y5, double fraction)
        {
            if ((Math.Abs(y1 - y2) > 300) || (Math.Abs(y2 - y3) > 300) || (Math.Abs(y3 - y4) > 300) || (Math.Abs(y4 - y5) > 300))
            {
                y1 = GCMath.putIn180(y1);
                y2 = GCMath.putIn180(y2);
                y3 = GCMath.putIn180(y3);
                y4 = GCMath.putIn180(y4);
                y5 = GCMath.putIn180(y5);
            }
            double A = y2 - y1;
            double B = y3 - y2;
            double C = y4 - y3;
            double D = y5 - y4;
            double E = B - A;
            double F = C - B;
            double G = D - C;
            double H = F - E;
            double J = G - F;
            double K = J - H;

            double n = fraction;
            double n2 = n * n;
            double n3 = n2 * n;
            double n4 = n2 * n2;

            return y3 + n * ((B + C) / 2 - (H + J) / 12) + n2 * (F / 2 - K / 24) + n3 * ((H + J) / 12) + n4 * K / 24;
        }
    }

    public class GCAstronomyCoordPage
    {
        public const int PAGE_SIZE = 40;
        public const int PAGE_HEAD = 2;
        public const int PAGE_TAIL = 2;
        public const int PAGE_ITEMS = PAGE_SIZE + PAGE_HEAD + PAGE_TAIL;

        /// <summary>
        /// First dimension  : bodyId: 0-earth, 1-moon, 2-mars, 3-mercury, 4-jupiter, 5-venus, 6-saturn, 7-rahu, 8-ketu
        /// Second dimension : time [0] - day before starting date, [1] - starting date, ....
        ///      in total there are data for 40 days + 3 values for interpolation needs
        /// Third dimension  : [0] - longitude, [1] - latitude, [2] - radius
        /// </summary>
        public double[,,] data = new double[9,PAGE_ITEMS,3];

        public bool[] dataInit = new bool[9];

        public int startDay = 0;

        public GCAstronomyCoordPage(int start)
        {
            for (int i = 0; i < 9; i++)
            {
                dataInit[i] = false;
                for (int j = 0; j < PAGE_ITEMS; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        data[i, j, k] = 0;
                    }
                }
            }

            // initiazilaction for earth data
            // we need this in any case
            startDay = start;
            InitializePlanet(0);
        }

        private delegate void CalcFuncDelegate(double jd, out double l, out double b, out double r);

        public void InitializePlanet(int bodyId)
        {
            dataInit[bodyId] = true;
            int jk = 0;
            CalcFuncDelegate method = GetCalcMethod(bodyId);
            int MIN = startDay - PAGE_HEAD;
            int MAX = startDay + PAGE_SIZE + PAGE_TAIL;
            for (int i = MIN; i < MAX; i++)
            {
                method(i, out data[bodyId, jk, 0], out data[bodyId, jk, 1], out data[bodyId, jk, 2]);
                jk++;
            }

            if (bodyId == 0)
            {
                ConvertEarthToSun();
            }
            else if (bodyId >= 2 && bodyId <= 6)
            {
                ConvertHeliocentricToGeocentric(bodyId);
            }
        }

        /// <summary>
        /// Converting earth's heliocentric coordinates into sun's geocentric coordinates
        /// </summary>
        private void ConvertEarthToSun()
        {
            for (int i = 0; i < PAGE_ITEMS; i++)
            {
                data[0, i, 0] = GCMath.putIn360(data[0, i, 0] + 180.0);
                data[0, i, 1] = -data[0, i, 1];
            }
        }

        /// <summary>
        /// Converting body's heliocentric coordinates into body's geocentric coordinates
        /// This method works only for Mars, Mercury, Jupiter, Venus and Saturn
        /// Other planets have their coordinates calculated in geocentric coordinate system already
        /// </summary>
        /// <param name="bodyId">ID of the celestial body</param>
        private void ConvertHeliocentricToGeocentric(int bodyId)
        {
            if (bodyId <= 1 || bodyId >= 7)
                return;

            for (int i = 0; i < PAGE_ITEMS; i++)
            {
                double longitudalDifference = data[bodyId, i, 0] - data[0, i, 0] - 180;
                double geocentricLongitude = data[bodyId, i, 0] + GCMath.arcTan2Deg(GCMath.sinDeg(longitudalDifference), data[bodyId,i,2] - GCMath.cosDeg(longitudalDifference));
                double sx = data[0, i, 2] * GCMath.sinDeg(data[0, i, 0]);
                double sy = data[0, i, 2] * GCMath.cosDeg(data[0, i, 0]);
                double sz = data[0, i, 2] * GCMath.sinDeg(data[0, i, 1]);
                double sq = data[0, i, 2] * GCMath.cosDeg(data[0, i, 1]);

                double vx = data[bodyId, i, 2] * GCMath.sinDeg(data[bodyId, i, 0]);
                double vy = data[bodyId, i, 2] * GCMath.cosDeg(data[bodyId, i, 0]);
                double vz = data[bodyId, i, 2] * GCMath.sinDeg(data[bodyId, i, 1]);
                double vq = data[bodyId, i, 2] * GCMath.cosDeg(data[bodyId, i, 1]);

                data[bodyId, i, 0] = GCMath.putIn360(geocentricLongitude);
                //data[bodyId, i, 0] = GCMath.putIn360(GCMath.arcTan2Deg(sy + vy, sx + vx));
                data[bodyId, i, 1] = GCMath.arcTan2Deg(sz + vz, sq + vq);
                data[bodyId, i, 2] = Math.Sqrt((sy + vy) * (sy + vy) + (sx + vx) * (sx + vx));
            }
        }

        private CalcFuncDelegate GetCalcMethod(int bodyId)
        {
            switch (bodyId)
            {
                case 1:
                    return new CalcFuncDelegate(GCMoonData.Calculate);
                case 2:
                    return new CalcFuncDelegate(GCMars.Calculate);
                case 3:
                    return new CalcFuncDelegate(GCMercury.Calculate);
                case 4:
                    return new CalcFuncDelegate(GCJupiter.Calculate);
                case 5:
                    return new CalcFuncDelegate(GCVenus.Calculate);
                case 6:
                    return new CalcFuncDelegate(GCSaturn.Calculate);
                case 7:
                    return new CalcFuncDelegate(GCRahu.Calculate);
                case 8:
                    return new CalcFuncDelegate(GCKetu.Calculate);
                default:
                    return new CalcFuncDelegate(GCEarth.Calculate);
            }
        }
    }
}
