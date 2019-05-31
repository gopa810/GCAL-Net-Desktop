using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base
{
    public class GCTithi
    {

        /*********************************************************************/
        /*                                                                   */
        /*   finds next time when starts next tithi                          */
        /*                                                                   */
        /*   timezone is not changed                                         */
        /*                                                                   */
        /*   return value: index of tithi 0..29                              */
        /*                 or -1 if failed                                   */
        /*********************************************************************/

        public static int GetNextTithiStart(GCEarthData ed, GregorianDateTime startDate, out GregorianDateTime nextDate)
        {
            double phi = 12.0;
            double l1, l2, longitudeSun, longitudeMoon;
            double jday = startDate.GetJulianComplete();
            double xj;
            GregorianDateTime d = new GregorianDateTime();
            d.Set(startDate);
            GregorianDateTime xd = new GregorianDateTime();
            double scan_step = 0.5;
            int prev_tit = 0;
            int new_tit = -1;

            longitudeSun = GCCoreAstronomy.GetSunLongitude(d, ed);
            longitudeMoon = GCCoreAstronomy.GetMoonLongitude(d, ed);
            l1 = GCMath.putIn360(longitudeMoon - longitudeSun - 180.0);
            prev_tit = GCMath.IntFloor(l1 / phi);

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

                longitudeSun = GCCoreAstronomy.GetSunLongitude(d, ed);
                longitudeMoon = GCCoreAstronomy.GetMoonLongitude(d, ed);
                l2 = GCMath.putIn360(longitudeMoon - longitudeSun - 180.0);
                new_tit = GCMath.IntFloor(l2 / phi);

                if (prev_tit != new_tit)
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
            nextDate = d;
            return new_tit;
        }

        /*********************************************************************/
        /*                                                                   */
        /*   finds previous time when starts next tithi                      */
        /*                                                                   */
        /*   timezone is not changed                                         */
        /*                                                                   */
        /*   return value: index of tithi 0..29                              */
        /*                 or -1 if failed                                   */
        /*********************************************************************/

        public static int GetPrevTithiStart(GCEarthData ed, GregorianDateTime startDate, out GregorianDateTime nextDate)
        {
            double phi = 12.0;
            double l1, l2, longitudeSun, longitudeMoon;
            double jday = startDate.GetJulianComplete();
            double xj;
            GregorianDateTime d = new GregorianDateTime();
            d.Set(startDate);
            GregorianDateTime xd = new GregorianDateTime();
            double scan_step = 0.5;
            int prev_tit = 0;
            int new_tit = -1;

            longitudeMoon = GCCoreAstronomy.GetMoonLongitude(d, ed);
            longitudeSun = GCCoreAstronomy.GetSunLongitude(d, ed);
            l1 = GCMath.putIn360(longitudeMoon - longitudeSun - 180.0);
            prev_tit = GCMath.IntFloor(l1 / phi);

            int counter = 0;
            while (counter < 20)
            {
                xj = jday;
                xd.Set(d);

                jday -= scan_step;
                d.shour -= scan_step;
                if (d.shour < 0.0)
                {
                    d.shour += 1.0;
                    d.PreviousDay();
                }

                longitudeSun = GCCoreAstronomy.GetSunLongitude(d, ed);
                longitudeMoon = GCCoreAstronomy.GetMoonLongitude(d, ed);
                l2 = GCMath.putIn360(longitudeMoon - longitudeSun - 180.0);
                new_tit = GCMath.IntFloor(l2 / phi);

                if (prev_tit != new_tit)
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
            nextDate = d;
            //	nextDate.shour += startDate.tzone / 24.0;
            //	nextDate.NormalizeValues();
            return new_tit;
        }

        /*

        Routines for calculation begining and ending times of given Tithi

        Main function is GetTithiTimes

        */

        public static double GetTithiTimes(GCEarthData ed, GregorianDateTime vc, out double titBeg, out double titEnd, double sunRise)
        {
            GregorianDateTime d1, d2;

            vc.shour = sunRise;
            GCTithi.GetPrevTithiStart(ed, vc, out d1);
            GCTithi.GetNextTithiStart(ed, vc, out d2);

            titBeg = d1.shour + d1.GetJulian() - vc.GetJulian();
            titEnd = d2.shour + d2.GetJulian() - vc.GetJulian();

            return (titEnd - titBeg);
        }

        /*********************************************************************/
        /* Finds starting and ending time for given tithi                    */
        /*                                                                   */
        /* tithi is specified by Gaurabda year, masa, paksa and tithi number */
        /*      nGYear - 0..9999                                             */
        /*       nMasa - 0..12, 0-Madhusudana, 1-Trivikrama, 2-Vamana        */
        /*                      3-Sridhara, 4-Hrsikesa, 5-Padmanabha         */
        /*                      6-Damodara, 7-Kesava, 8-narayana, 9-Madhava  */
        /*                      10-Govinda, 11-Visnu, 12-PurusottamaAdhika   */
        /*       nPaksa -       0-Krsna, 1-Gaura                             */
        /*       nTithi - 0..14                                              */
        /*       earth  - used timezone                                      */
        /*                                                                   */
        /*********************************************************************/

        public static GregorianDateTime CalcTithiEnd(int nGYear, int nMasa, int nPaksa, int nTithi, GCEarthData earth, out GregorianDateTime endTithi)
        {
            GregorianDateTime d = new GregorianDateTime();

            d.Set(GCAstroData.GetFirstDayOfYear(earth, nGYear + 1486));
            d.shour = 0.5;
            d.TimezoneHours = earth.OffsetUtcHours;

            return CalcTithiEndEx(d, nGYear, nMasa, nPaksa, nTithi, earth, out endTithi);
        }

        public static GregorianDateTime CalcTithiEndEx(GregorianDateTime vcStart, int GYear, int nMasa, int nPaksa, int nTithi, GCEarthData earth, out GregorianDateTime endTithi)
        {
            int i, gy, nType;
            GregorianDateTime d = new GregorianDateTime();
            GCAstroData day = new GCAstroData();
            int tithi;
            int counter;
            GregorianDateTime start = new GregorianDateTime(), end = new GregorianDateTime();
            //	SUNDATA sun;
            //	MOONDATA moon;
            double sunrise;
            start.shour = -1.0;
            end.shour = -1.0;
            start.day = start.month = start.year = -1;
            end.day = end.month = end.year = -1;

            /*	d = GetFirstDayOfYear(earth, nGYear + 1486);
                d.shour = 0.5;
                d.TimeZone = earth.tzone;
            */
            d.Set(vcStart);

            i = 0;
            do
            {
                d.AddDays(13);
                day.DayCalc(d, earth);
                day.Masa = day.MasaCalc(d, earth);
                gy = day.GaurabdaYear;
                i++;
            }
            while (((day.sunRise.Paksa != nPaksa) || (day.Masa != nMasa)) && (i <= 30));

            if (i >= 30)
            {
                d.year = d.month = d.day = -1;
                endTithi = null;
                return d;
            }

            // we found masa and paksa
            // now we have to find tithi
            tithi = nTithi + nPaksa * 15;

            if (day.sunRise.Tithi == tithi)
            {
                // loc1
                // find tithi juncts in this day and according to that times,
                // look in previous or next day for end and start of this tithi
                nType = 1;
            }
            else
            {
                if (day.sunRise.Tithi < tithi)
                {
                    // do increment of date until nTithi == tithi
                    //   but if nTithi > tithi
                    //       then do decrement of date
                    counter = 0;
                    while (counter < 30)
                    {
                        d.NextDay();
                        day.DayCalc(d, earth);
                        if (day.sunRise.Tithi == tithi)
                            goto cont_2;
                        if ((day.sunRise.Tithi < tithi) && (day.sunRise.Paksa != nPaksa))
                        {
                            d.PreviousDay();
                            goto cont_2;
                        }
                        if (day.sunRise.Tithi > tithi)
                        {
                            d.PreviousDay();
                            goto cont_2;
                        }
                        counter++;
                    }
                    // somewhere is error
                    d.year = d.month = d.day = 0;
                    nType = 0;
                }
                else
                {
                    // do decrement of date until nTithi <= tithi
                    counter = 0;
                    while (counter < 30)
                    {
                        d.PreviousDay();
                        day.DayCalc(d, earth);
                        if (day.sunRise.Tithi == tithi)
                            goto cont_2;
                        if ((day.sunRise.Tithi > tithi) && (day.sunRise.Paksa != nPaksa))
                        {
                            goto cont_2;
                        }
                        if (day.sunRise.Tithi < tithi)
                        {
                            goto cont_2;
                        }
                        counter++;
                    }
                    // somewhere is error
                    d.year = d.month = d.day = 0;
                    nType = 0;

                }
            cont_2:
                if (day.sunRise.Tithi == tithi)
                {
                    // do the same as in loc1
                    nType = 1;
                }
                else
                {
                    // nTithi != tithi and nTithi < tithi
                    // but on next day is nTithi > tithi
                    // that means we will find start and the end of tithi
                    // in this very day or on next day before sunrise
                    nType = 2;
                }

            }

            // now we know the type of day-accurancy
            // nType = 0 means, that we dont found a day
            // nType = 1 means, we find day, when tithi was present at sunrise
            // nType = 2 means, we found day, when tithi started after sunrise
            //                  but ended before next sunrise
            //
            sunrise = day.sunRise.TotalDays;
            endTithi = new GregorianDateTime();
            if (nType == 1)
            {
                GregorianDateTime d1, d2;
                d.shour = sunrise;
                GCTithi.GetPrevTithiStart(earth, d, out d1);
                //d = d1;
                //d.shour += 0.02;
                GCTithi.GetNextTithiStart(earth, d, out d2);
                endTithi.Set(d2);
                return d1;
            }
            else if (nType == 2)
            {
                GregorianDateTime d1, d2;
                d.shour = sunrise;
                GCTithi.GetNextTithiStart(earth, d, out d1);
                d.Set(d1);
                d.shour += 0.1;
                d.NormalizeValues();
                GCTithi.GetNextTithiStart(earth, d, out d2);

                endTithi.Set(d2);
                return d1;
            }

            // if nType == 0, then this algoritmus has some failure
            if (nType == 0)
            {
                d.year = 0;
                d.month = 0;
                d.day = 0;
                d.shour = 0.0;
                endTithi.Set(d);
            }
            else
            {
                d.Set(start);
                endTithi.Set(end);
            }
            return d;
        }


        /*********************************************************************/
        /*  Calculates Date of given Tithi                                   */
        /*********************************************************************/

        public static GregorianDateTime CalcTithiDate(int nGYear, int nMasa, int nPaksa, int nTithi, GCEarthData earth)
        {
            int i = 0, gy = 0;
            GregorianDateTime d = new GregorianDateTime();
            GCAstroData day = new GCAstroData();
            int tithi = 0;
            int counter = 0;
            UInt32 tmp = 0;

            if (nGYear >= 464 && nGYear < 572)
            {
                tmp = GCAstroData.gGaurBeg[(nGYear - 464) * 26 + nMasa * 2 + nPaksa];
                d.month = Convert.ToInt32((tmp & 0x3e0) >> 5);
                d.day = Convert.ToInt32(tmp & 0x1f);
                d.year = Convert.ToInt32(tmp & 0xfffc00) >> 10;
                d.TimezoneHours = earth.OffsetUtcHours;
                d.NextDay();

                day.DayCalc(d, earth);
                day.Masa = day.MasaCalc(d, earth);
                gy = day.GaurabdaYear;
            }
            else
            {
                //d = GetFirstDayOfYear(earth, nGYear + 1486);
                d.day = 15;
                d.month = 2 + nMasa;
                d.year = nGYear + 1486;
                if (d.month > 12)
                {
                    d.month -= 12;
                    d.year++;
                }
                d.shour = 0.5;
                d.TimezoneHours = earth.OffsetUtcHours;

                i = 0;
                do
                {
                    d.AddDays(13);
                    day.DayCalc(d, earth);
                    day.Masa = day.MasaCalc(d, earth);
                    gy = day.GaurabdaYear;
                    i++;
                }
                while (((day.sunRise.Paksa != nPaksa) || (day.Masa != nMasa)) && (i <= 30));
            }

            if (i >= 30)
            {
                d.year = d.month = d.day = -1;
                return d;
            }

            // we found masa and paksa
            // now we have to find tithi
            tithi = nTithi + nPaksa * 15;

            if (day.sunRise.Tithi == tithi)
            {
                // loc1
                // find tithi juncts in this day and according to that times,
                // look in previous or next day for end and start of this tithi
                d.PreviousDay();
                day.DayCalc(d, earth);
                if ((day.sunRise.Tithi > tithi) && (day.sunRise.Paksa != nPaksa))
                {
                    d.NextDay();
                }
                return d;
            }

            if (day.sunRise.Tithi < tithi)
            {
                // do increment of date until nTithi == tithi
                //   but if nTithi > tithi
                //       then do decrement of date
                counter = 0;
                while (counter < 16)
                {
                    d.NextDay();
                    day.DayCalc(d, earth);
                    if (day.sunRise.Tithi == tithi)
                        return d;
                    if ((day.sunRise.Tithi < tithi) && (day.sunRise.Paksa != nPaksa))
                        return d;
                    if (day.sunRise.Tithi > tithi)
                        return d;
                    counter++;
                }
                // somewhere is error
                d.year = d.month = d.day = 0;
                return d;
            }
            else
            {
                // do decrement of date until nTithi <= tithi
                counter = 0;
                while (counter < 16)
                {
                    d.PreviousDay();
                    day.DayCalc(d, earth);
                    if (day.sunRise.Tithi == tithi)
                        return d;
                    if ((day.sunRise.Tithi > tithi) && (day.sunRise.Paksa != nPaksa))
                    {
                        d.NextDay();
                        return d;
                    }
                    if (day.sunRise.Tithi < tithi)
                    {
                        d.NextDay();
                        return d;
                    }
                    counter++;
                }
                // somewhere is error
                d.year = d.month = d.day = 0;
                return d;
            }

            // now we know the type of day-accurancy
            // nType = 0 means, that we dont found a day
            // nType = 1 means, we find day, when tithi was present at sunrise
            // nType = 2 means, we found day, when tithi started after sunrise
            //                  but ended before next sunrise
            //

            return d;
        }

        public static int writeXml(string fileName, GCLocation loc, GregorianDateTime vc)
        {
            String str;
            GregorianDateTime date;

            using (StreamWriter xml = new StreamWriter(fileName))
            {

                xml.Write("<xml>\n");
                xml.Write("\t<request name=\"Tithi\" version=\"");
                xml.Write(GCStrings.getString(130));
                xml.Write("\">\n");
                xml.Write("\t\t<arg name=\"longitude\" val=\"");
                xml.Write(loc.Longitude);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"latitude\" val=\"");
                xml.Write(loc.Latitude);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"timezone\" val=\"");
                xml.Write(loc.OffsetUtcHours);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"startdate\" val=\"");
                xml.Write(vc);
                xml.Write("\" />\n");
                xml.Write("\t</request>\n");
                xml.Write("\t<result name=\"Tithi\">\n");

                GregorianDateTime d = new GregorianDateTime();
                d.Set(vc);
                GregorianDateTime d1, d2;
                d.TimezoneHours = loc.OffsetUtcHours;
                GregorianDateTime dn;
                GCHourTime dt = new GCHourTime();
                GCEarthData earth = loc.GetEarthData();


                GCAstroData day = new GCAstroData();

                day.DayCalc(vc, earth);

                d.shour = day.sunRise.TotalDays;

                GCTithi.GetPrevTithiStart(earth, d, out d1);
                GCTithi.GetNextTithiStart(earth, d, out d2);

                {
                    dt.SetDegTime(d1.shour * 360);
                    // start tithi at t[0]
                    xml.Write("\t\t<tithi\n\t\t\tid=\"");
                    xml.Write(day.sunRise.Tithi);
                    xml.Write("\"\n");
                    xml.Write("\t\t\tname=\"");
                    xml.Write(GCTithi.GetName(day.sunRise.Tithi));
                    xml.Write("\"\n");
                    xml.Write("\t\t\tstartdate=\"");
                    xml.Write(d1);
                    xml.Write("\"\n");
                    xml.Write("\t\t\tstarttime=\"");
                    xml.Write(dt);
                    xml.Write("\"\n");

                    dt.SetDegTime(d2.shour * 360);
                    xml.Write("\t\t\tenddate=\"");
                    xml.Write(d2);
                    xml.Write("\"\n");
                    xml.Write("\t\t\tendtime=\"");
                    xml.Write(dt);
                    xml.Write("\"\n />");
                }

                xml.Write("\t</result>\n");
                xml.Write("</xml>\n");
            }
            return 1;
        }



        public static int writeGaurabdaTithiXml(string fileName, GCLocation loc, GaurabdaDate vaStart, GaurabdaDate vaEnd)
        {
            int gyearA = vaStart.gyear;
            int gyearB = vaEnd.gyear;
            int gmasa = vaStart.masa;
            int gpaksa = vaStart.tithi / 15;
            int gtithi = vaStart.tithi % 15;

            if (gyearB < gyearA)
                gyearB = gyearA;

            using (StreamWriter xml = new StreamWriter(fileName))
            {

                xml.Write("<xml>\n");
                xml.Write("\t<request name=\"Tithi\" version=\"");
                xml.Write(GCStrings.getString(130));
                xml.Write("\">\n");
                xml.Write("\t\t<arg name=\"longitude\" val=\"");
                xml.Write(loc.Longitude);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"latitude\" val=\"");
                xml.Write(loc.Latitude);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"timezone\" val=\"");
                xml.Write(loc.OffsetUtcHours);
                xml.Write("\" />\n");
                if (gyearA > 1500)
                {
                    xml.Write("\t\t<arg name=\"year-start\" val=\"");
                    xml.Write(gyearA);
                    xml.Write("\" />\n");
                    xml.Write("\t\t<arg name=\"year-end\" val=\"");
                    xml.Write(gyearB);
                    xml.Write("\" />\n");
                }
                else
                {
                    xml.Write("\t\t<arg name=\"gaurabdayear-start\" val=\"");
                    xml.Write(gyearA);
                    xml.Write("\" />\n");
                    xml.Write("\t\t<arg name=\"gaurabdayear-end\" val=\"");
                    xml.Write(gyearB);
                    xml.Write("\" />\n");
                }
                xml.Write("\t\t<arg name=\"masa\" val=\"");
                xml.Write(gmasa);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"paksa\" val=\"");
                xml.Write(gpaksa);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"tithi\" val=\"");
                xml.Write(gtithi);
                xml.Write("\" />\n");
                xml.Write("\t</request>\n");
                xml.Write("\t<result name=\"Tithi\">\n");


                GCEarthData earth = loc.GetEarthData();
                GregorianDateTime vcs = new GregorianDateTime(), vce = new GregorianDateTime(), today = new GregorianDateTime();
                GCHourTime sun = new GCHourTime();
                int A, B;
                double sunrise;
                GCAstroData day = new GCAstroData();
                int oTithi, oPaksa, oMasa, oYear;

                if (gyearA > 1500)
                {
                    A = gyearA - 1487;
                    B = gyearB - 1485;
                }
                else
                {
                    A = gyearA;
                    B = gyearB;
                }

                for (; A <= B; A++)
                {
                    vcs.Set(GCTithi.CalcTithiEnd(A, gmasa, gpaksa, gtithi, earth, out vce));
                    if (gyearA > 1500)
                    {
                        if ((vcs.year < gyearA) || (vcs.year > gyearB))
                            continue;
                    }
                    oTithi = gpaksa * 15 + gtithi;
                    oMasa = gmasa;
                    oPaksa = gpaksa;
                    oYear = 0;
                    xml.Write("\t<celebration\n");
                    //		xml.Write("\t\t<tithi\n";
                    xml.Write("\t\trtithi=\"");
                    xml.Write(GCTithi.GetName(oTithi));
                    xml.Write("\"\n");
                    xml.Write("\t\trmasa=\"");
                    xml.Write(GCMasa.GetName(oMasa));
                    xml.Write("\"\n");
                    xml.Write("\t\trpaksa=\"");
                    xml.Write(oPaksa != 0 ? "Gaura" : "Krsna");
                    xml.Write("\"\n");
                    // test ci je ksaya
                    today.Set(vcs);
                    today.shour = 0.5;
                    sun = GCSunData.CalcSunrise(today, earth);
                    sunrise = sun.TotalDays;
                    if (sunrise < vcs.shour)
                    {
                        today.Set(vce);
                        sun = GCSunData.CalcSunrise(today, earth);
                        sunrise = sun.TotalDays;
                        if (sunrise < vce.shour)
                        {
                            // normal type
                            vcs.NextDay();
                            xml.Write("\t\ttype=\"normal\"\n");
                        }
                        else
                        {
                            // ksaya
                            vcs.NextDay();
                            day.DayCalc(vcs, earth);
                            oTithi = day.sunRise.Tithi;
                            oPaksa = day.sunRise.Paksa;
                            oMasa = day.MasaCalc(vcs, earth);
                            oYear = day.GaurabdaYear;
                            xml.Write("\t\ttype=\"ksaya\"\n");
                        }
                    }
                    else
                    {
                        // normal, alebo prvy den vriddhi
                        today.Set(vce);
                        sun = GCSunData.CalcSunrise(today, earth);
                        if (sun.TotalDays < vce.shour)
                        {
                            // first day of vriddhi type
                            xml.Write("\t\ttype=\"vriddhi\"\n");
                        }
                        else
                        {
                            // normal
                            xml.Write("\t\ttype=\"normal\"\n");
                        }
                    }
                    xml.Write("\t\tdate=\"");
                    xml.Write(vcs);
                    xml.Write("\"\n");
                    xml.Write("\t\totithi=\"");
                    xml.Write(GCTithi.GetName(oTithi));
                    xml.Write("\"\n");
                    xml.Write("\t\tomasa=\"");
                    xml.Write(GCMasa.GetName(oMasa));
                    xml.Write("\"\n");
                    xml.Write("\t\topaksa=\"");
                    xml.Write(GCPaksa.GetName(oPaksa));
                    xml.Write("\"\n");
                    xml.Write("\t/>\n");
                }

                xml.Write("\t</result>\n");
                xml.Write("</xml>\n");

            }
            return 1;
        }

        public static int writeGaurabdaNextTithiXml(string fileName, GCLocation loc, GregorianDateTime vcStart, GaurabdaDate vaStart)
        {
            int gmasa, gpaksa, gtithi;
            GCHourTime sunRise;

            using (StreamWriter xml = new StreamWriter(fileName))
            {

                gmasa = vaStart.masa;
                gpaksa = vaStart.tithi / 15;
                gtithi = vaStart.tithi % 15;

                xml.Write("<xml>\n");
                xml.Write("\t<request name=\"Tithi\" version=\"");
                xml.Write(GCStrings.getString(130));
                xml.Write("\">\n");
                xml.Write("\t\t<arg name=\"longitude\" val=\"");
                xml.Write(loc.Longitude);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"latitude\" val=\"");
                xml.Write(loc.Latitude);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"timezone\" val=\"");
                xml.Write(loc.OffsetUtcHours);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"start date\" val=\"");
                xml.Write(vcStart);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"masa\" val=\"");
                xml.Write(gmasa);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"paksa\" val=\"");
                xml.Write(gpaksa);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"tithi\" val=\"");
                xml.Write(gtithi);
                xml.Write("\" />\n");
                xml.Write("\t</request>\n");
                xml.Write("\t<result name=\"Tithi\">\n");

                GCEarthData earth = loc.GetEarthData();
                GregorianDateTime vcs = new GregorianDateTime(), vce = new GregorianDateTime(), today = new GregorianDateTime();
                //GCSunData sun = new GCSunData();
                int A;
                double sunrise;
                GCAstroData day = new GCAstroData();
                int oTithi, oPaksa, oMasa, oYear;

                today.Set(vcStart);
                today.PreviousDay();
                vcStart.SubtractDays(15);
                for (A = 0; A <= 3; A++)
                {
                    vcs.Set(GCTithi.CalcTithiEndEx(vcStart, 0, gmasa, gpaksa, gtithi, earth, out vce));
                    if (!vcs.IsBeforeThis(today))
                    {
                        oTithi = gpaksa * 15 + gtithi;
                        oMasa = gmasa;
                        oPaksa = gpaksa;
                        oYear = 0;
                        xml.Write("\t<celebration\n");
                        //		xml.Write("\t\t<tithi\n";
                        xml.Write("\t\trtithi=\"");
                        xml.Write(GCTithi.GetName(oTithi));
                        xml.Write("\"\n");
                        xml.Write("\t\trmasa=\"");
                        xml.Write(GCMasa.GetName(oMasa));
                        xml.Write("\"\n");
                        xml.Write("\t\trpaksa=\"");
                        xml.Write((oPaksa != 0 ? "Gaura" : "Krsna"));
                        xml.Write("\"\n");
                        // test ci je ksaya
                        today.Set(vcs);
                        today.shour = 0.5;
                        sunRise = GCSunData.CalcSunrise(today, earth);
                        sunrise = sunRise.TotalDays;
                        if (sunrise < vcs.shour)
                        {
                            today.Set(vce);
                            sunRise = GCSunData.CalcSunrise(today, earth);
                            sunrise = sunRise.TotalDays;
                            if (sunrise < vce.shour)
                            {
                                // normal type
                                vcs.NextDay();
                                xml.Write("\t\ttype=\"normal\"\n");
                            }
                            else
                            {
                                // ksaya
                                vcs.NextDay();
                                day.DayCalc(vcs, earth);
                                oTithi = day.sunRise.Tithi;
                                oPaksa = day.sunRise.Paksa;
                                oMasa = day.MasaCalc(vcs, earth);
                                oYear = day.GaurabdaYear;
                                xml.Write("\t\ttype=\"ksaya\"\n");
                            }
                        }
                        else
                        {
                            // normal, alebo prvy den vriddhi
                            today.Set(vce);
                            sunRise = GCSunData.CalcSunrise(today, earth);
                            if (sunRise.TotalDays < vce.shour)
                            {
                                // first day of vriddhi type
                                xml.Write("\t\ttype=\"vriddhi\"\n");
                            }
                            else
                            {
                                // normal
                                xml.Write("\t\ttype=\"normal\"\n");
                            }
                        }
                        xml.Write("\t\tdate=\"");
                        xml.Write(vcs);
                        xml.Write("\"\n");
                        xml.Write("\t\totithi=\"");
                        xml.Write(GCTithi.GetName(oTithi));
                        xml.Write("\"\n");
                        xml.Write("\t\tomasa=\"");
                        xml.Write(GCMasa.GetName(oMasa));
                        xml.Write("\"\n");
                        xml.Write("\t\topaksa=\"");
                        xml.Write(GCPaksa.GetName(oPaksa));
                        xml.Write("\"\n");
                        xml.Write("\t/>\n");
                        break;
                    }
                    else
                    {
                        vcStart.Set(vcs);
                        vcs.NextDay();
                    }
                }

                xml.Write("\t</result>\n");
                xml.Write("</xml>\n");
            }

            return 1;
        }

        public static bool TITHI_EKADASI(int a)
        {
            return (((a) == 10) || ((a) == 25));
        }

        public static bool TITHI_DVADASI(int a)
        {
            return (((a) == 11) || ((a) == 26));
        }

        public static bool TITHI_TRAYODASI(int a)
        {
            return (((a) == 12) || ((a) == 27));
        }

        public static bool TITHI_CATURDASI(int a) { return ((a == 13) || (a == 28)); }

        public static bool TITHI_LESS_EKADASI(int a) { return (((a) == 9) || ((a) == 24) || ((a) == 8) || ((a) == 23)); }
        public static bool TITHI_LESS_DVADASI(int a) { return (((a) == 10) || ((a) == 25) || ((a) == 9) || ((a) == 24)); }
        public static bool TITHI_LESS_TRAYODASI(int a) { return (((a) == 11) || ((a) == 26) || ((a) == 10) || ((a) == 25)); }
        public static bool TITHI_FULLNEW_MOON(int a) { return (((a) == 14) || ((a) == 29)); }

        public static int PREV_PREV_TITHI(int a) { return (((a) + 28) % 30); }
        public static int PREV_TITHI(int a) { return (((a) + 29) % 30); }
        public static int NEXT_TITHI(int a) { return (((a) + 1) % 30); }
        public static int NEXT_NEXT_TITHI(int a) { return (((a) + 1) % 30); }

        public static bool TITHI_LESS_THAN(int a, int b) { return (((a) == PREV_TITHI(b)) || ((a) == PREV_PREV_TITHI(b))); }
        public static bool TITHI_GREAT_THAN(int a, int b) { return (((a) == NEXT_TITHI(b)) || ((a) == NEXT_NEXT_TITHI(b))); }

        // TRUE when transit from A to B is between T and U
        public static bool TITHI_TRANSIT(int t, int u, int a, int b)
        {
            return ((t == a) && (u == b))
                || ((t == a) && (u == NEXT_TITHI(b)))
                || ((t == PREV_TITHI(a)) && (u == b));
        }

        public static int TITHI_DIST(int a, int b)
        {
            int r = a - b;
            while (r < -15)
                r += 30;
            return r;
        }

        private static string[] p_tithiName = new string[] {
            "Pratipat", // GCStrings.Localized("Pratipat");
            "Dvitiya", // GCStrings.Localized("Dvitiya");
            "Tritiya", // GCStrings.Localized("Tritiya");
            "Caturthi", // GCStrings.Localized("Caturthi");
            "Pancami", // GCStrings.Localized("Pancami");
            "Sasti", // GCStrings.Localized("Sasti");
            "Saptami", // GCStrings.Localized("Saptami");
            "Astami", // GCStrings.Localized("Astami");
            "Navami", // GCStrings.Localized("Navami");
            "Dasami", // GCStrings.Localized("Dasami");
            "Ekadasi", // GCStrings.Localized("Ekadasi");
            "Dvadasi", // GCStrings.Localized("Dvadasi");
            "Trayodasi", // GCStrings.Localized("Trayodasi");
            "Caturdasi", // GCStrings.Localized("Caturdasi");
            "Amavasya", // GCStrings.Localized("Amavasya");
            "Pratipat", // GCStrings.Localized("Pratipat");
            "Dvitiya", // GCStrings.Localized("Dvitiya");
            "Tritiya", // GCStrings.Localized("Tritiya");
            "Caturthi", // GCStrings.Localized("Caturthi");
            "Pancami", // GCStrings.Localized("Pancami");
            "Sasti", // GCStrings.Localized("Sasti");
            "Saptami", // GCStrings.Localized("Saptami");
            "Astami", // GCStrings.Localized("Astami");
            "Navami", // GCStrings.Localized("Navami");
            "Dasami", // GCStrings.Localized("Dasami");
            "Ekadasi", // GCStrings.Localized("Ekadasi");
            "Dvadasi", // GCStrings.Localized("Dvadasi");
            "Trayodasi", // GCStrings.Localized("Trayodasi");
            "Caturdasi", // GCStrings.Localized("Caturdasi");
            "Purnima" // GCStrings.Localized("Purnima");
        };

        /// <summary>
        /// Retrieves name of tithi
        /// </summary>
        /// <param name="nTithi">Values 0..29, where 0 is for Pratipat in Krsna Paksa</param>
        /// <returns></returns>
        public static string GetName(int nTithi)
        {
            return GCStrings.Localized(p_tithiName[nTithi % 30]);
        }
    }
}
