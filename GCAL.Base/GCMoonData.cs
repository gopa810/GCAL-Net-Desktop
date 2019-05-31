using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCMoonData
    {
        // latitude from nodal (draconic) phase
        public double latitude_deg;
        // longitude from sidereal motion
        public double longitude_deg;

        public double radius; //(* lambda, beta, R *)
        public double rightAscension, declination;  //(* alpha, delta *)
        public double parallax;
        public double elevation, azimuth;//          (* h, A *)




        public void Calculate(double jdate)
        {

            GCEclipticalCoords crd = CalculateEcliptical(jdate);
            GCEquatorialCoords eqc;

            eqc = GCEarthData.eclipticalToEquatorialCoords(ref crd, jdate);

            this.radius = crd.distance;
            this.longitude_deg = crd.longitude;
            this.latitude_deg = crd.latitude;

            // equaltorial coordinates
            this.rightAscension = eqc.rightAscension;
            this.declination = eqc.declination;
        }

        public static void Calculate(double jd, out double L, out double B, out double R)
        {
            GCMoonData md = new GCMoonData();
            md.Calculate(jd);
            L = md.longitude_deg;
            B = md.latitude_deg;
            R = md.radius;
        }

        //==================================================================================
        //
        //==================================================================================

        public static void CalcMoonTimes(GCEarthData e, GregorianDateTime vc, double biasHours, out GCHourTime rise, out GCHourTime set)
        {
            double UT;
            int i;
            double prev_elev;
            int nType, nFound = 0;
            double a, ae, b, be, c = 0, ce = 0, elev;

            rise = new GCHourTime();
            set = new GCHourTime();

            rise.SetValue(-1);
            set.SetValue(-1);

            // inicializacia prvej hodnoty ELEVATION
            vc.shour = (-biasHours - 1.0) / 24.0;
            prev_elev = GCCoreAstronomy.GetMoonElevation(e, vc);

            // prechod cez vsetky hodiny
            for (UT = (-0.1 - biasHours); UT <= (24.1 - biasHours); UT += 1.0)
            {
                vc.shour = UT / 24.0;
                elev = GCCoreAstronomy.GetMoonElevation(e, vc);

                if (prev_elev * elev <= 0.0)
                {
                    if (prev_elev <= 0.0)
                    {
                        nType = 0x1;
                    }
                    else
                    {
                        nType = 0x2;
                    }

                    a = UT - 1.0;
                    ae = prev_elev;
                    b = UT;
                    be = elev;
                    for (i = 0; i < 20; i++)
                    {
                        c = (a + b) / 2.0;
                        vc.shour = c / 24.0;
                        ce = GCCoreAstronomy.GetMoonElevation(e, vc);
                        if (ae * ce <= 0.0)
                        {
                            be = ce;
                            b = c;
                        }
                        else
                        {
                            ae = ce;
                            a = c;
                        }
                    }

                    if (nType == 1)
                    {
                        rise.SetDayTime((c + biasHours) / 24.0);
                    }
                    else
                    {
                        set.SetDayTime((c + biasHours) / 24.0);
                    }

                    nFound |= nType;

                    if (nFound == 0x3)
                        break;
                }

                prev_elev = elev;
            }

        }

        public static int GetNextMoonRasi(GCEarthData ed, GregorianDateTime startDate, out GregorianDateTime nextDate)
        {
            double phi = 30.0;
            double l1, l2, longitudeMoon;
            double jday = startDate.GetJulianComplete();
            GregorianDateTime d = new GregorianDateTime();
            d.Set(startDate);
            double ayanamsa = GCAyanamsha.GetAyanamsa(jday);
            double scan_step = 0.5;
            int prev_naks = 0;
            int new_naks = -1;

            double xj;
            GregorianDateTime xd = new GregorianDateTime();

            longitudeMoon = GCCoreAstronomy.GetMoonLongitude(d, ed);
            l1 = GCMath.putIn360(longitudeMoon - ayanamsa);
            prev_naks = GCMath.IntFloor(l1 / phi);

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

                longitudeMoon = GCCoreAstronomy.GetMoonLongitude(d, ed);
                l2 = GCMath.putIn360(longitudeMoon - ayanamsa);
                new_naks = GCMath.IntFloor(l2 / phi);
                if (prev_naks != new_naks)
                {
                    jday = xj;
                    d.Set(xd);
                    scan_step *= 0.5;
                    counter++;
                    continue;
                }
                else
                {
                    l1 = l2;
                }
            }
            nextDate = new GregorianDateTime();
            nextDate.Set(d);
            return new_naks;
        }

        //==================================================================================
        //
        //==================================================================================

        public static GregorianDateTime GetNextRise(GCEarthData e, GregorianDateTime vc, bool bRise)
        {
            double a, b;

            double[] h = new double[3];
            double hour = 1 / 24.0;
            double startHour = vc.shour;

            GregorianDateTime track = new GregorianDateTime();
            track.Set(vc);
            track.NormalizeValues();

            // inicializacia prvej hodnoty ELEVATION
            h[0] = GCCoreAstronomy.GetMoonElevation(e, track);
            track.shour += hour;
            h[1] = GCCoreAstronomy.GetMoonElevation(e, track);
            track.shour += hour;
            h[2] = GCCoreAstronomy.GetMoonElevation(e, track);

            for (int c = 0; c < 24; c++)
            {
                if ((bRise ? (h[1] < 0.0 && h[2] > 0.0) : (h[1] > 0.0 && h[2] < 0.0)))
                {
                    a = (h[2] - h[1]) / hour;
                    b = h[2] - a * track.shour;
                    track.shour = -b / a;
                    track.NormalizeValues();
                    return track;
                }

                h[0] = h[1];
                h[1] = h[2];
                track.shour += hour;
                h[2] = GCCoreAstronomy.GetMoonElevation(e, track);
            }

            return track;
        }


        //==================================================================================
        //
        //==================================================================================



        public void calc_horizontal(double date, double longitude, double latitude)
        {
            double h;

            h = GCMath.putIn360(GCEarthData.SiderealTimeGreenwich(date) - this.rightAscension + longitude);

            this.azimuth = GCMath.rad2deg(Math.Atan2(GCMath.sinDeg(h),
                                   GCMath.cosDeg(h) * GCMath.sinDeg(latitude) -
                                   GCMath.tanDeg(this.declination) * GCMath.cosDeg(latitude)));

            this.elevation = GCMath.rad2deg(Math.Asin(GCMath.sinDeg(latitude) * GCMath.sinDeg(this.declination) +
                                    GCMath.cosDeg(latitude) * GCMath.cosDeg(this.declination) * GCMath.cosDeg(h)));
        }

        public GCEquatorialCoords getTopocentricEquatorial(GCEarthData obs, double jdate)
        {
            double u, h, delta_alpha;
            double rho_sin, rho_cos;
            const double b_a = 0.99664719;
            GCEquatorialCoords tec = new GCEquatorialCoords();

            double altitude = 0;

            // geocentric position of observer on the earth surface
            // 10.1 - 10.3
            u = GCMath.arcTanDeg(b_a * b_a * GCMath.tanDeg(obs.latitudeDeg));
            rho_sin = b_a * GCMath.sinDeg(u) + altitude / 6378140.0 * GCMath.sinDeg(obs.latitudeDeg);
            rho_cos = GCMath.cosDeg(u) + altitude / 6378140.0 * GCMath.cosDeg(obs.latitudeDeg);

            // equatorial horizontal paralax
            // 39.1
            double parallax = GCMath.arcSinDeg(GCMath.sinDeg(8.794 / 3600) / (radius / GCMath.AU));

            // geocentric hour angle of the body
            h = GCEarthData.SiderealTimeGreenwich(jdate) + obs.longitudeDeg - rightAscension;


            // 39.2
            delta_alpha = GCMath.arcTanDeg(
                        (-rho_cos * GCMath.sinDeg(parallax) * GCMath.sinDeg(h)) /
                        (GCMath.cosDeg(this.declination) - rho_cos * GCMath.sinDeg(parallax) * GCMath.cosDeg(h)));
            tec.rightAscension = rightAscension + delta_alpha;
            tec.declination = declination + GCMath.arcTanDeg(
                ((GCMath.sinDeg(declination) - rho_sin * GCMath.sinDeg(parallax)) * GCMath.cosDeg(delta_alpha)) /
                (GCMath.cosDeg(declination) - rho_cos * GCMath.sinDeg(parallax) * GCMath.cosDeg(h)));

            return tec;
        }


        public void CorrectEqatorialWithParallax(double jdate, double latitude, double longitude, double height)
        {
            double u, hourAngleBody, delta_alpha;
            double rho_sin, rho_cos;
            const double b_a = 0.99664719;

            // calculate geocentric longitude and latitude of observer
            u = GCMath.arcTanDeg(b_a * b_a * GCMath.tanDeg(latitude));
            rho_sin = b_a * GCMath.sinDeg(u) + height / 6378140.0 * GCMath.sinDeg(latitude);
            rho_cos = GCMath.cosDeg(u) + height / 6378140.0 * GCMath.cosDeg(latitude);

            // calculate paralax
            this.parallax = GCMath.arcSinDeg(GCMath.sinDeg(8.794 / 3600) / (MoonDistance(jdate) / GCMath.AU));

            // calculate correction of equatorial coordinates
            hourAngleBody = GCEarthData.SiderealTimeGreenwich(jdate) + longitude - this.rightAscension;
            delta_alpha = GCMath.arcTan2Deg(-rho_cos * GCMath.sinDeg(this.parallax) * GCMath.sinDeg(hourAngleBody),
                        GCMath.cosDeg(this.declination) - rho_cos * GCMath.sinDeg(this.parallax) * GCMath.cosDeg(hourAngleBody));
            this.rightAscension += delta_alpha;
            this.declination += GCMath.arcTan2Deg(  
                (GCMath.sinDeg(this.declination) - rho_sin * GCMath.sinDeg(this.parallax)) * GCMath.cosDeg(delta_alpha) ,
                 GCMath.cosDeg(this.declination) - rho_cos * GCMath.sinDeg(this.parallax) * GCMath.cosDeg(hourAngleBody));
        }

        private static int[,] arg_lr = {
		 { 0, 0, 1, 0},		 { 2, 0,-1, 0},		 { 2, 0, 0, 0},		 { 0, 0, 2, 0},		 { 0, 1, 0, 0},
		 { 0, 0, 0, 2},		 { 2, 0,-2, 0},		 { 2,-1,-1, 0},		 { 2, 0, 1, 0},		 { 2,-1, 0, 0},
		 { 0, 1,-1, 0},		 { 1, 0, 0, 0},		 { 0, 1, 1, 0},		 { 2, 0, 0,-2},		 { 0, 0, 1, 2},
		 { 0, 0, 1,-2},		 { 4, 0,-1, 0},		 { 0, 0, 3, 0},		 { 4, 0,-2, 0},		 { 2, 1,-1, 0},
		 { 2, 1, 0, 0},		 { 1, 0,-1, 0},		 { 1, 1, 0, 0},		 { 2,-1, 1, 0},		 { 2, 0, 2, 0},
		 { 4, 0, 0, 0},		 { 2, 0,-3, 0},		 { 0, 1,-2, 0},		 { 2, 0,-1, 2},		 { 2,-1,-2, 0},
		 { 1, 0, 1, 0},		 { 2,-2, 0, 0},		 { 0, 1, 2, 0},		 { 0, 2, 0, 0},		 { 2,-2,-1, 0},
		 { 2, 0, 1,-2},		 { 2, 0, 0, 2},		 { 4,-1,-1, 0},		 { 0, 0, 2, 2},		 { 3, 0,-1, 0},
		 { 2, 1, 1, 0},		 { 4,-1,-2, 0},		 { 0, 2,-1, 0},		 { 2, 2,-1, 0},		 { 2, 1,-2, 0},
		 { 2,-1, 0,-2},		 { 4, 0, 1, 0},		 { 0, 0, 4, 0},		 { 4,-1, 0, 0},		 { 1, 0,-2, 0},
		 { 2, 1, 0,-2},		 { 0, 0, 2,-2},		 { 1, 1, 1, 0},		 { 3, 0,-2, 0},		 { 4, 0,-3, 0},
		 { 2,-1, 2, 0},		 { 0, 2, 1, 0},		 { 1, 1,-1, 0},		 { 2, 0, 3, 0},		 { 2, 0,-1,-2}
	   };

        private static int[,] arg_b = {
		 { 0, 0, 0, 1},	 { 0, 0, 1, 1},	 { 0, 0, 1,-1},	 { 2, 0, 0,-1},
		 { 2, 0,-1, 1},	 { 2, 0,-1,-1},	 { 2, 0, 0, 1},	 { 0, 0, 2, 1},
		 { 2, 0, 1,-1},	 { 0, 0, 2,-1},  { 2,-1, 0,-1},	 { 2, 0,-2,-1},
		 { 2, 0, 1, 1},	 { 2, 1, 0,-1},	 { 2,-1,-1, 1},	 { 2,-1, 0, 1},
		 { 2,-1,-1,-1},	 { 0, 1,-1,-1},	 { 4, 0,-1,-1},	 { 0, 1, 0, 1},
		 { 0, 0, 0, 3},	 { 0, 1,-1, 1},	 { 1, 0, 0, 1},	 { 0, 1, 1, 1},
		 { 0, 1, 1,-1},	 { 0, 1, 0,-1},	 { 1, 0, 0,-1},	 { 0, 0, 3, 1},
		 { 4, 0, 0,-1},	 { 4, 0,-1, 1},	 { 0, 0, 1,-3},	 { 4, 0,-2, 1},
		 { 2, 0, 0,-3},	 { 2, 0, 2,-1},	 { 2,-1, 1,-1},	 { 2, 0,-2, 1},
		 { 0, 0, 3,-1},	 { 2, 0, 2, 1},	 { 2, 0,-3,-1},	 { 2, 1,-1, 1},
		 { 2, 1, 0, 1},	 { 4, 0, 0, 1},	 { 2,-1, 1, 1},	 { 2,-2, 0,-1},
		 { 0, 0, 1, 3},	 { 2, 1, 1,-1},	 { 1, 1, 0,-1},	 { 1, 1, 0, 1},
		 { 0, 1,-2,-1},	 { 2, 1,-1,-1},	 { 1, 0, 1, 1},	 { 2,-1,-2,-1},
		 { 0, 1, 2, 1},	 { 4, 0,-2,-1},	 { 4,-1,-1,-1},	 { 1, 0, 1,-1},
		 { 4, 0, 1,-1},	 { 1, 0,-1,-1},	 { 4,-1, 0,-1},	 { 2,-2, 0, 1}
		};

        private static int[] sigma_r = {
	   -20905355,		-3699111,		-2955968,		 -569925,		   48888,		   -3149,		  246158,
		 -152138,		 -170733,		 -204586,		 -129620,		  108743,		  104755,		   10321,
			   0,		   79661,		  -34782,		  -23210,		  -21636,		   24208,		   30824,
		   -8379,		  -16675,		  -12831,		  -10445,		  -11650,		   14403,		   -7003,
			   0,		   10056,			6322,		   -9884,			5751,			   0,		   -4950,
			4130,			   0,		   -3958,			   0,			3258,			2616,		   -1897,
		   -2117,			2354,			   0,			   0,		   -1423,		   -1117,		   -1571,
		   -1739,			   0,		   -4421,			   0,			   0,			   0,			   0,
			1165,			   0,			   0,			8752
				  };

        private static int[] sigma_l = {
		6288774,		1274027,		 658314,		 213618,
		-185116,		-114332,		  58793,		  57066,
		  53322,		  45758,		 -40923,		 -34720,
		 -30383,		  15327,		 -12528,		  10980,
		  10675,		  10034,		   8548,		  -7888,
		  -6766,		  -5163,		   4987,		   4036,
		   3994,		   3861,		   3665,		  -2689,
		  -2602,		   2390,		  -2348,		   2236,
		  -2120,		  -2069,		   2048,		  -1773,
		  -1595,		   1215,		  -1110,		   -892,
		   -810,			759,		   -713,		   -700,
		    691,			596,			549,			537,
			520,		   -487,		   -399,		   -381,
			351,		   -340,			330,			327,
		   -323,			299,			294,			  0
		};
        private static int[] sigma_b = {
		5128122,		 280602,		 277693,		 173237,
		  55413,		  46271,		  32573,		  17198,
		   9266,		   8822,		   8216,		   4324,
		   4200,		  -3359,		   2463,		   2211,
		   2065,		  -1870,		   1828,		  -1794,
		  -1749,		  -1565,		  -1491,		  -1475,
		  -1410,		  -1344,		  -1335,		   1107,
		   1021,			833,			777,			671,
			607,			596,			491,		   -451,
			439,			422,			421,		   -366,
		   -351,			331,			315,			302,
		   -283,		   -229,			223,			223,
		   -220,		   -220,		   -185,			181,
		   -177,			176,			166,		   -164,
			132,		   -119,			115,			107
		};

        public static double MoonDistance(double jdate)
        {

            double temp, t, d, m, ms, f, e, ls, sr;// : extended;

            t = (jdate - 2451545.0) / 36525.0;

            //(* mean elongation of the moon  
            d = 297.8502042 + (445267.1115168 + (-0.0016300 + (1.0 / 545868 - 1.0 / 113065000 * t) * t) * t) * t;

            //(* mean anomaly of the sun  
            m = 357.5291092 + (35999.0502909 + (-0.0001536 + 1.0 / 24490000 * t) * t) * t;

            //(* mean anomaly of the moon  
            ms = 134.9634114 + (477198.8676313 + (0.0089970 + (1.0 / 69699 - 1.0 / 1471200 * t) * t) * t) * t;

            //(* argument of the longitude of the moon  
            f = 93.2720993 + (483202.0175273 + (-0.0034029 + (-1.0 / 3526000 + 1.0 / 863310000 * t) * t) * t) * t;

            //(* correction term due to excentricity of the earth orbit  
            e = 1.0 + (-0.002516 - 0.0000074 * t) * t;

            //(* mean longitude of the moon  
            ls = 218.3164591 + (481267.88134236 + (-0.0013268 + (1.0 / 538841 - 1.0 / 65194000 * t) * t) * t) * t;

            sr = 0;

            int i;
            for (i = 0; i < 60; i++)
            {
                temp = sigma_r[i] * GCMath.cosDeg(arg_lr[i, 0] * d
                                       + arg_lr[i, 1] * m
                                       + arg_lr[i, 2] * ms
                                       + arg_lr[i, 3] * f);
                if (Math.Abs(arg_lr[i, 1]) == 1) temp = temp * e;
                if (Math.Abs(arg_lr[i, 1]) == 2) temp = temp * e * e;
                sr = sr + temp;
            }


            return 385000.56 + sr / 1000;

        }



        public static GCEclipticalCoords CalculateEcliptical(double jdate)
        {
            double t, d, m, ms, f, e, ls;
            double sr, sl, sb, temp;// : extended;
            double a1, a2, a3; // : extended;
            int i; //: integer;


            t = (jdate - 2451545.0) / 36525.0;

            //(* mean elongation of the moon  
            d = 297.8502042 + (445267.1115168 + (-0.0016300 + (1.0 / 545868 - 1.0 / 113065000 * t) * t) * t) * t;

            //(* mean anomaly of the sun  
            m = 357.5291092 + (35999.0502909 + (-0.0001536 + 1.0 / 24490000 * t) * t) * t;

            //(* mean anomaly of the moon  
            ms = 134.9634114 + (477198.8676313 + (0.0089970 + (1.0 / 69699 - 1.0 / 1471200 * t) * t) * t) * t;

            //(* argument of the longitude of the moon  
            f = 93.2720993 + (483202.0175273 + (-0.0034029 + (-1.0 / 3526000 + 1.0 / 863310000 * t) * t) * t) * t;

            //(* correction term due to excentricity of the earth orbit  
            e = 1.0 + (-0.002516 - 0.0000074 * t) * t;

            //(* mean longitude of the moon  
            ls = 218.3164591 + (481267.88134236 + (-0.0013268 + (1.0 / 538841 - 1.0 / 65194000 * t) * t) * t) * t;

            //(* arguments of correction terms  
            a1 = 119.75 + 131.849 * t;
            a2 = 53.09 + 479264.290 * t;
            a3 = 313.45 + 481266.484 * t;

            sr = 0;
            for (i = 0; i < 60; i++)
            {
                temp = sigma_r[i] * GCMath.cosDeg(arg_lr[i, 0] * d
                                       + arg_lr[i, 1] * m
                                       + arg_lr[i, 2] * ms
                                       + arg_lr[i, 3] * f);
                if (Math.Abs(arg_lr[i, 1]) == 1) temp = temp * e;
                if (Math.Abs(arg_lr[i, 1]) == 2) temp = temp * e * e;
                sr = sr + temp;
            }

            sl = 0;
            for (i = 0; i < 60; i++)
            {
                temp = sigma_l[i] * GCMath.sinDeg(arg_lr[i, 0] * d
                                       + arg_lr[i, 1] * m
                                       + arg_lr[i, 2] * ms
                                       + arg_lr[i, 3] * f);
                if (Math.Abs(arg_lr[i, 1]) == 1) temp = temp * e;
                if (Math.Abs(arg_lr[i, 1]) == 2) temp = temp * e * e;
                sl = sl + temp;
            }

            //(* correction terms  
            sl = sl + 3958 * GCMath.sinDeg(a1)
                + 1962 * GCMath.sinDeg(ls - f)
                + 318 * GCMath.sinDeg(a2);
            sb = 0;
            for (i = 0; i < 60; i++)
            {
                temp = sigma_b[i] * GCMath.sinDeg(arg_b[i, 0] * d
                                       + arg_b[i, 1] * m
                                       + arg_b[i, 2] * ms
                                       + arg_b[i, 3] * f);
                if (Math.Abs(arg_b[i, 1]) == 1) temp = temp * e;
                if (Math.Abs(arg_b[i, 1]) == 2) temp = temp * e * e;
                sb = sb + temp;
            }

            //(* correction terms  
            sb = sb - 2235 * GCMath.sinDeg(ls)
                  + 382 * GCMath.sinDeg(a3)
                  + 175 * GCMath.sinDeg(a1 - f)
                  + 175 * GCMath.sinDeg(a1 + f)
                  + 127 * GCMath.sinDeg(ls - ms)
                  - 115 * GCMath.sinDeg(ls + ms);

            GCEclipticalCoords coords;

            coords.longitude = ls + sl / 1000000;
            coords.latitude = sb / 1000000;
            coords.distance = 385000.56 + sr / 1000;

            return coords;
        }

    }
}
