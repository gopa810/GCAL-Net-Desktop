using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCMath
    {
        // pi
        public static readonly double PI = 3.1415926535897932385;
        // 2 * pi
        public static readonly double PI2 = 6.2831853071795864769;
        // pi / 180
        public static readonly double RADS = 0.0174532925199432958;

        public static readonly double AU = 149597869.0;

        public static readonly double EARTH_RADIUS = 6378.15;     // Radius of the earth 
        public static readonly double MOON_RADIUS = 1737.4;
        public static readonly double SUN_RADIUS = 695500;
        public static readonly double J1999 = 2451180.0;
        public static readonly double J2000 = 2451545.0;


        /////////////////////////////////////
        // input value: arc in degrees

        public static double cosDeg(double x)
        {
            return Math.Cos(x * RADS);
        }

        /////////////////////////////////////
        // input value: arc in degrees

        public static double sinDeg(double x)
        {
            return Math.Sin(x * RADS);
        }

        public static double arccosDeg(double x)
        {
            return Math.Acos(x) / RADS;
        }

        public static double Abs(double d)
        {
            return Math.Abs(d);
        }

        /////////////////////////////////////
        // input value: arc in degrees
        // it is calculating arctan(x/y)
        // with testing values

        public static double arcTan2Deg(double y, double x)
        {
            return Math.Atan2(y, x) / RADS;
        }

        /////////////////////////////////////
        // input value: arc in degrees
        // output value: tangens 

        public static double tanDeg(double x)
        {
            return Math.Tan(x * RADS);
        }

        /////////////////////////////////////
        // input value: -1.0 .. 1.0
        // output value: -180 deg .. 180 deg

        public static double arcSinDeg(double x)
        {
            return Math.Asin(x) / RADS;
        }



        public static double arcTanDeg(double x)
        {
            return Math.Atan(x) / RADS;
        }

        // modulo 1

        public static double putIn1(double v)
        {
            double v2 = v - Math.Floor(v);
            while (v2 < 0.0)
                v2 += 1.0;
            while (v2 > 1.0)
                v2 -= 1.0;
            return v2;
        }

        public static double putIn24(double id)
        {
            double d = id;
            while (d >= 24.0)
                d -= 24.0;
            while (d < 0.0)
                d += 24.0;
            return d;
        }

        // modulo 360

        public static double putIn360(double id)
        {
            double d = id;
/*            if (d < 0.0)
                d += Math.Floor(-d / 360.0) * 360.0;
            if (d > 360.0)
                d -= Math.Floor(d / 360.0) * 360;*/
            while (d >= 360.0)
                d -= 360.0;
            while (d < 0.0)
                d += 360.0;
            return d;
        }

        // modulo 360 but in range -180deg .. 180deg
        // used for comparision values around 0deg
        // so difference between 359deg and 1 deg 
        // is not 358 degrees, but 2 degrees (because 359deg = -1 deg)

        public static double putIn180(double in_d)
        {
            double d = in_d;

            while (d < -180.0)
            {
                d += 360.0;
            }
            while (d > 180.0)
            {
                d -= 360.0;
            }

            return d;
        }



        // sign of the number
        // -1: number is less than zero
        // 0: number is zero
        // +1: number is greater than zero

        public static int getSign(double d)
        {
            if (d > 0.0)
                return 1;
            if (d < 0.0)
                return -1;
            return 0;
        }

        public static double deg2rad(double x)
        {
            return x * RADS;
        }

        public static double rad2deg(double x)
        {
            return x / RADS;
        }

        public static double getFraction(double x)
        {
            return x - Math.Floor(x);
        }

        public static double Max(double a, double b)
        {
            if (a > b)
                return a;
            return b;
        }

        public static double Min(double a, double b)
        {
            if (a < b)
                return a;
            return b;
        }

        public static double arcDistance(double lon1, double lat1, double lon2, double lat2)
        {
            lat1 = PI / 2 - lat1;
            lat2 = PI / 2 - lat2;
            return arccosDeg(cosDeg(lat1) * cosDeg(lat2) + sinDeg(lat1) * sinDeg(lat2) * cosDeg(lon1 - lon2));
        }

        public static double arcDistanceDeg(double lon1, double lat1, double lon2, double lat2)
        {
            return rad2deg(arcDistance(deg2rad(lon1), deg2rad(lat1), deg2rad(lon2), deg2rad(lat2)));
        }

        public static double Floor(double d)
        {
            return Math.Floor(d);
        }

        public static int IntFloor(double d)
        {
            return Convert.ToInt32(Math.Floor(d));
        }

        public static int IntRound(double d)
        {
            return Convert.ToInt32(Math.Round(d));
        }

        public static void DaytimeToHourMin(double shour, out int hour, out int minute)
        {
            shour *= 24.0;
            hour = IntFloor(shour);
            shour -= hour;
            shour *= 60;
            minute = IntRound(shour);
        }

        public static void DaytimeToHourMin(double shour, out int hour, out int minute, out int sec)
        {
            shour *= 24.0;
            hour = IntFloor(shour);
            shour -= hour;
            shour *= 60;
            minute = IntFloor(shour);
            shour -= minute;
            shour *= 60;
            sec = IntRound(shour);
        }

        public static TCoreEvent Min(TCoreEvent A, TCoreEvent B)
        {
            if (A.Time < B.Time)
                return A;
            else
                return B;
        }

        public static TCoreEvent Max(TCoreEvent A, TCoreEvent B)
        {
            return (A.Time > B.Time) ? A : B;
        }
    }
}
