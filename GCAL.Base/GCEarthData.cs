using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GCAL.Base
{

    public struct GCHorizontalCoords
    {
        public double azimut;
        public double elevation;
    };

    public struct GCEquatorialCoords
    {
        public double rightAscension;
        public double declination;
    };

    public struct GCEclipticalCoords
    {
        public double latitude;
        public double longitude;
        public double distance;
    };

    public class GCEarthData
    {
        private string p_timezonename = "";
        private TTimeZone p_timezone = null;
        public string TimeZoneName { get { return p_timezonename; } set { p_timezonename = value; p_timezone = null; } }
        public TTimeZone TimeZone
        {
            get
            {
                if (p_timezone != null) return p_timezone;
                p_timezone = TTimeZone.FindTimeZoneByName(p_timezonename);
                return p_timezone;
            }
            set
            {
                p_timezone = value;
                p_timezonename = (value == null ? "" : p_timezone.Name);
            }
        }

        // terrestrial longitude of observation
        public double longitudeDeg;

        // terrestrial latitude of observation
        public double latitudeDeg;

        // time zone (hours)
        public double OffsetUtcHours
        {
            get
            {
                if (TimeZone == null)
                    return 0.0;
                return TimeZone.OffsetMinutes / 60.0;
            }
        }

        // observated event
        // 0 - center of the sun
        // 1 - civil twilight
        // 2 - nautical twilight
        // 3 - astronomical twilight
        public int obs;


        public GCEarthData()
        {
            obs = 0;
            longitudeDeg = 0.0;
            latitudeDeg = 0.0;
            TimeZone = TTimeZone.GetDefaultTimeZone();
        }

        private static int[,] arg_mul = {
		                                    { 0, 0, 0, 0, 1},
		                                    {-2, 0, 0, 2, 2},
		                                    { 0, 0, 0, 2, 2},
		                                    { 0, 0, 0, 0, 2},
		                                    { 0, 1, 0, 0, 0},
		                                    { 0, 0, 1, 0, 0},
		                                    {-2, 1, 0, 2, 2},
		                                    { 0, 0, 0, 2, 1},
		                                    { 0, 0, 1, 2, 2},
		                                    {-2,-1, 0, 2, 2},
		                                    {-2, 0, 1, 0, 0},
		                                    {-2, 0, 0, 2, 1},
		                                    { 0, 0,-1, 2, 2},
		                                    { 2, 0, 0, 0, 0},
		                                    { 0, 0, 1, 0, 1},
		                                    { 2, 0,-1, 2, 2},
		                                    { 0, 0,-1, 0, 1},
		                                    { 0, 0, 1, 2, 1},
		                                    {-2, 0, 2, 0, 0},
		                                    { 0, 0,-2, 2, 1},
		                                    { 2, 0, 0, 2, 2},
		                                    { 0, 0, 2, 2, 2},
		                                    { 0, 0, 2, 0, 0},
		                                    {-2, 0, 1, 2, 2},
		                                    { 0, 0, 0, 2, 0},
		                                    {-2, 0, 0, 2, 0},
		                                    { 0, 0,-1, 2, 1},
		                                    { 0, 2, 0, 0, 0},
		                                    { 2, 0,-1, 0, 1},
		                                    {-2, 2, 0, 2, 2},
		                                    { 0, 1, 0, 0, 1}
                                        };
        private static int[,] arg_phi = {
		    {-171996,-1742},
		    { -13187,  -16},
		    {  -2274,   -2},
		    {   2062,    2},
		    {   1426,  -34},
		    {    712,    1},
		    {   -517,   12},
		    {   -386,   -4},
		    {   -301,    0},
		    {    217,   -5},
		    {   -158,    0},
		    {    129,    1},
		    {    123,    0},
		    {     63,    0},
		    {     63,    1},
		    {    -59,    0},
		    {    -58,   -1},
		    {    -51,    0},
		    {     48,    0},
		    {     46,    0},
		    {    -38,    0},
		    {    -31,    0},
		    {     29,    0},
		    {     29,    0},
		    {     26,    0},
		    {    -22,    0},
		    {     21,    0},
		    {     17,   -1},
		    {     16,    0},
		    {    -16,    1},
		    {    -15,    0}
		};
        private static int[,] arg_eps = {
		    { 92025,   89},
		    {  5736,  -31},
		    {   977,   -5},
		    {  -895,    5},
		    {    54,   -1},
		    {    -7,    0},
		    {   224,   -6},
		    {   200,    0},
		    {   129,   -1},
		    {   -95,    3},
		    {     0,    0},
		    {   -70,    0},
		    {   -53,    0},
		    {     0,    0},
		    {   -33,    0},
		    {    26,    0},
		    {    32,    0},
		    {    27,    0},
		    {     0,    0},
		    {   -24,    0},
		    {    16,    0},
		    {    13,    0},
		    {     0,    0},
		    {   -12,    0},
		    {     0,    0},
		    {     0,    0},
		    {   -10,    0},
		    {     0,    0},
		    {    -8,    0},
		    {     7,    0},
		    {     9,    0}
	    };

        public static bool LowPrecisionNutations = true;

        /// <summary>
        /// This is implementation from Chapter 21, Astronomical Algorithms
        /// Nutation of longitude and Obliquity of ecliptic
        /// </summary>
        /// <param name="julianDateUTC"></param>
        /// <param name="nutationLongitude"></param>
        /// <param name="obliquity"></param>
        public static void CalculateNutations(double julianDateUTC, out double nutationLongitude, out double obliquity)
        {
            double t, omega;
            double d, m, ms, f, s, l, ls;
            int i;
            double meanObliquity, nutationObliquity;

            t = (julianDateUTC - 2451545.0) / 36525;

            // longitude of rising knot
            omega = GCMath.putIn360(125.04452 + (-1934.136261 + (0.0020708 + 1.0 / 450000 * t) * t) * t);

            if (LowPrecisionNutations)
            {
                // (*@/// delta_phi and delta_epsilon - low accuracy *)
                //(* mean longitude of sun (l) and moon (ls) *)
                l = 280.4665 + 36000.7698 * t;
                ls = 218.3165 + 481267.8813 * t;

                //(* correction due to nutation *)
                nutationObliquity = 9.20 * GCMath.cosDeg(omega) + 0.57 * GCMath.cosDeg(2 * l) + 0.10 * GCMath.cosDeg(2 * ls) - 0.09 * GCMath.cosDeg(2 * omega);

                //(* longitude correction due to nutation *)
                nutationLongitude = (-17.20 * GCMath.sinDeg(omega) - 1.32 * GCMath.sinDeg(2 * l) - 0.23 * GCMath.sinDeg(2 * ls) + 0.21 * GCMath.sinDeg(2 * omega)) / 3600;
            }
            else
            {
                // mean elongation of moon to sun
                d = GCMath.putIn360(297.85036 + (445267.111480 + (-0.0019142 + t / 189474) * t) * t);

                // mean anomaly of the sun
                m = GCMath.putIn360(357.52772 + (35999.050340 + (-0.0001603 - t / 300000) * t) * t);

                // mean anomaly of the moon
                ms = GCMath.putIn360(134.96298 + (477198.867398 + (0.0086972 + t / 56250) * t) * t);

                // argument of the latitude of the moon
                f = GCMath.putIn360(93.27191 + (483202.017538 + (-0.0036825 + t / 327270) * t) * t);

                nutationLongitude = 0;
                nutationObliquity = 0;

                for (i = 0; i < 31; i++)
                {
                    s = arg_mul[i, 0] * d
                       + arg_mul[i, 1] * m
                       + arg_mul[i, 2] * ms
                       + arg_mul[i, 3] * f
                       + arg_mul[i, 4] * omega;
                    nutationLongitude = nutationLongitude + (arg_phi[i, 0] + arg_phi[i, 1] * t * 0.1) * GCMath.sinDeg(s);
                    nutationObliquity = nutationObliquity + (arg_eps[i, 0] + arg_eps[i, 1] * t * 0.1) * GCMath.cosDeg(s);
                }

                nutationLongitude = nutationLongitude * 0.0001 / 3600;
                nutationObliquity = nutationObliquity * 0.0001;
            }
            // angle of ecliptic
            meanObliquity = 84381.448 + (-46.8150 + (-0.00059 + 0.001813 * t) * t) * t;

            obliquity = (meanObliquity + nutationObliquity) / 3600;

        }


        public static GCEquatorialCoords eclipticalToEquatorialCoords(ref GCEclipticalCoords ecc, double date)
        {
            //var
            GCEquatorialCoords eqc;

            double epsilon;
            double nutationLongitude;

            GCEarthData.CalculateNutations(date, out nutationLongitude, out epsilon);


            // formula from Chapter 21
            ecc.longitude = GCMath.putIn360(ecc.longitude + nutationLongitude);

            // formulas from Chapter 12
            eqc.rightAscension = GCMath.arcTan2Deg(GCMath.sinDeg(ecc.longitude) * GCMath.cosDeg(epsilon) - GCMath.tanDeg(ecc.latitude) * GCMath.sinDeg(epsilon), 
                GCMath.cosDeg(ecc.longitude));

            eqc.declination = GCMath.arcSinDeg(GCMath.sinDeg(ecc.latitude) * GCMath.cosDeg(epsilon) + GCMath.cosDeg(ecc.latitude) * GCMath.sinDeg(epsilon) * GCMath.sinDeg(ecc.longitude));

            return eqc;
        }

        public static GCHorizontalCoords equatorialToHorizontalCoords(GCEquatorialCoords eqc, GCEarthData obs, double date)
        {
	        double localHourAngle;
	        GCHorizontalCoords hc;

	        localHourAngle = GCMath.putIn360(GCEarthData.SiderealTimeGreenwich(date) - eqc.rightAscension + obs.longitudeDeg);
	
	        hc.azimut = GCMath.rad2deg( Math.Atan2(GCMath.sinDeg(localHourAngle),
                                   GCMath.cosDeg(localHourAngle)*GCMath.sinDeg(obs.latitudeDeg)-
                                   GCMath.tanDeg(eqc.declination)*GCMath.cosDeg(obs.latitudeDeg) ));
	
	        hc.elevation = GCMath.rad2deg(Math.Asin(GCMath.sinDeg(obs.latitudeDeg)*GCMath.sinDeg(eqc.declination) +
                                    GCMath.cosDeg(obs.latitudeDeg)*GCMath.cosDeg(eqc.declination)*GCMath.cosDeg(localHourAngle)));

	        return hc;
        }

        public static bool ToDouble(string s, out double val, char poschar, char negchar)
        {
            int i;

            i = s.IndexOf(negchar);
            if (i >= 0)
                return ToDouble2(s, i, -1.0, out val);
            i = s.IndexOf(poschar);
            if (i >= 0)
                return ToDouble2(s, i, 1.0, out val);

            val = 0.0;
            return false;
        }

        private static bool ToDouble2(string s, int i, double sig, out double val)
        {
            int a, b;
            if (int.TryParse(s.Substring(0, i), out a))
            {
                if (int.TryParse(s.Substring(i + 1), out b))
                {
                    val = sig * (a * 60.0 + b * 1.0) / 60.0;
                    return true;
                }
            }

            val = 0.0;
            return false;
        }
        public static string GetTextLatitude(double d)
        {
            int a0, a1;
            char c0;

            c0 = d < 0.0 ? 'S' : 'N';
            d = Math.Abs(d);
            a0 = GCMath.IntFloor(d);
            a1 = GCMath.IntFloor((d - a0) * 60 + 0.5);

            return string.Format("{0}{1}{2:00}", a0, c0, a1);
        }

        public static string GetTextLongitude(double d)
        {
            int a0, a1;
            char c0;

            c0 = d < 0.0 ? 'W' : 'E';
            d = Math.Abs(d);
            a0 = GCMath.IntFloor(d);
            a1 = GCMath.IntFloor((d - a0) * 60 + 0.5);

            return string.Format("{0}{1}{2:00}", a0, c0, a1);
        }

        public static double SiderealTimeGreenwich(double date)
        {
            double jd, t;
            double delta_phi = 0.0, epsilon = 0.0;

            jd = date;
            t = (jd - 2451545.0) / 36525.0;
            GCEarthData.CalculateNutations(date, out delta_phi, out epsilon);
            return GCMath.putIn360(280.46061837 + 360.98564736629 * (jd - 2451545.0) +
                             t * t * (0.000387933 - t / 38710000) +
                             delta_phi * GCMath.cosDeg(epsilon));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="julianDateUTC">Time in UT1 or for general use UTC. This should reflect also hours, minutes and seconds. When finding local sidereal time, 
        /// this argument should contains time that is observed on Greenwich meridian. Use function GetJulianDetailed or GetGreenwichDateTime</param>
        /// <param name="longitudeDegrees"></param>
        /// <returns></returns>
        public static double SiderealTimeLocal(double julianDateUTC, double longitudeDegrees, double timezoneDegrees)
        {
            double julianDate2000, julianMillenium;
            double delta_phi = 0.0, epsilon = 0.0;

            julianDate2000 = julianDateUTC - 2451545.0;
            julianMillenium = julianDate2000 / 36525.0;
            GCEarthData.CalculateNutations(julianDateUTC, out delta_phi, out epsilon);
            return GCMath.putIn360(280.46061837 + 360.98564736629 * julianDate2000 +
                             julianMillenium * julianMillenium * (0.000387933 - julianMillenium / 38710000) +
                             delta_phi * GCMath.cosDeg(epsilon) + longitudeDegrees - timezoneDegrees);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="julianDateUTC">This contains time UTC, that means time observed on Greenwich meridian. DateTime in other
        /// timezones should be converted into timezone UTC+0h before using in this method.</param>
        /// <returns></returns>
        public double GetAscendantDegrees(double julianDateUTC)
        {
            double A = GCEarthData.SiderealTimeLocal(julianDateUTC, longitudeDeg, OffsetUtcHours*15.0);
            double E = 23.4392911;
            double ascendant = GCMath.arcTan2Deg(-GCMath.cosDeg(A), GCMath.sinDeg(A)*GCMath.cosDeg(E) + GCMath.tanDeg(latitudeDeg)*GCMath.sinDeg(E));
            if (ascendant < 180)
                ascendant += 180;
            else
                ascendant -= 180;
            return GCMath.putIn360(ascendant  - GCAyanamsha.GetAyanamsa(julianDateUTC));
        }

        public int GetNextAscendentStart(GregorianDateTime startDate, out GregorianDateTime nextDate)
        {
            int countOfElements = 12;
            double phi = 360.0 / countOfElements;
            double l1, l2;
            double jday = startDate.GetJulianDetailed();
            double xj;
            GregorianDateTime d = new GregorianDateTime(startDate);
            // we are calculating in UTC timezone
            d.shour -= startDate.TimezoneHours / 24.0;
            d.TimezoneHours = 0.0;
            GregorianDateTime xd = new GregorianDateTime();
            double scan_step = 1.0 / 48.0;
            int prev_tit = 0;
            int new_tit = -1;

            l1 = GetAscendantDegrees(jday);

            prev_tit = Convert.ToInt32(Math.Floor(l1 / phi));
            //Debugger.Log(0, "", "=== FROM " + startDate.LongTimeString() + " ===\n");
            //Debugger.Log(0, "", string.Format("{0:00}: {1} {2}     {3} {4} \n", 99, l1, prev_tit, jday, d.LongTimeString()));
            int counter = 0;
            while (counter < 20)
            {
                xj = jday;
                xd.Set(d);

                jday += scan_step;
                d.shour += scan_step;
                if (d.shour > 1.0)
                {
                    d.shour -= 1.0;
                    d.NextDay();
                }

                l2 = GetAscendantDegrees(jday);
                new_tit = Convert.ToInt32(Math.Floor(l2 / phi));
                //Debugger.Log(0, "", string.Format("{0:00}: {1} {2}     {3} {4} \n", counter, l1, prev_tit, jday, d.LongTimeString()));

                if (prev_tit != new_tit)
                {
                    jday = xj;
                    d.Set(xd);
                    scan_step *= 0.5;
                    counter++;
                    //Debugger.Log(0, "", string.Format("   Going back to {0}\n", d.LongTimeString()));
                    continue;
                }
                else
                {
                    l1 = l2;
                }
            }
            // date D was calculated in timezone UTC+0h, therefore
            // we have to convert it into timezone of input datetime
            nextDate = new GregorianDateTime(d);
            nextDate.shour += startDate.TimezoneHours / 24.0;
            nextDate.TimezoneHours = startDate.TimezoneHours;
            nextDate.NormalizeValues();

            return new_tit;
        }



        public override string ToString()
        {
            return string.Format("{0}: {1}  {2}: {3}  {4}: {5}",
                GCStrings.getString(10), GCEarthData.GetTextLatitude(latitudeDeg),
                GCStrings.getString(11), GCEarthData.GetTextLongitude(longitudeDeg),
                GCStrings.Localized("Timezone"), TTimeZone.GetTimeZoneOffsetText(OffsetUtcHours));

        }

    }
}
