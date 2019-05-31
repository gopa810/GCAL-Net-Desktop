using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base
{
    public class GCNaksatra
    {
        public const int N_ASVINI = 0;
        public const int N_BHARANI = 1;
        public const int N_KRITTIKA = 2;
        public const int N_ROHINI = 3;
        public const int N_MRIGASIRA = 4;
        public const int N_ARDRA = 5;
        public const int N_PUNARVASU = 6;
        public const int N_PUSYAMI = 7;
        public const int N_ASHLESA = 8;
        public const int N_MAGHA = 9;
        public const int N_SRAVANA = 21;
        /*********************************************************************/
        /*                                                                   */
        /*   finds next time when starts next naksatra                       */
        /*                                                                   */
        /*   timezone is not changed                                         */
        /*                                                                   */
        /*   return value: index of naksatra 0..26                           */
        /*                 or -1 if failed                                   */
        /*********************************************************************/

        public static int GetNextNaksatra(GCEarthData ed, GregorianDateTime startDate, out GregorianDateTime nextDate)
        {
            double phi = 40.0 / 3.0;
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
            nextDate = d;
            return new_naks;
        }

        /*********************************************************************/
        /*                                                                   */
        /*   finds previous time when starts next naksatra                   */
        /*                                                                   */
        /*   timezone is not changed                                         */
        /*                                                                   */
        /*   return value: index of naksatra 0..26                           */
        /*                 or -1 if failed                                   */
        /*********************************************************************/

        public static int GetPrevNaksatra(GCEarthData ed, GregorianDateTime startDate, out GregorianDateTime nextDate)
        {
            double phi = 40.0 / 3.0;
            double l1, l2, longitudeMoon;
            double jday = startDate.GetJulianComplete();
            GregorianDateTime d = new GregorianDateTime(startDate);
            double xj;
            GregorianDateTime xd = new GregorianDateTime();
            double ayanamsa = GCAyanamsha.GetAyanamsa(jday);
            double scan_step = 0.5;
            int prev_naks = 0;
            int new_naks = -1;


            longitudeMoon = GCCoreAstronomy.GetMoonLongitude(d, ed);
            l1 = GCMath.putIn360(longitudeMoon - ayanamsa);
            prev_naks = GCMath.IntFloor(l1 / phi);

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

            nextDate = d;
            return new_naks;

        }


        public static int writeXml(string fileName, GCLocation loc, GregorianDateTime vc, int nDaysCount)
        {
            using (StreamWriter xml = new StreamWriter(fileName))
            {
                xml.Write("<xml>\n");
                xml.Write("\t<request name=\"Naksatra\" version=\"");
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
                xml.Write("\t\t<arg name=\"daycount\" val=\"");
                xml.Write(nDaysCount);
                xml.Write("\" />\n");
                xml.Write("\t</request>\n");
                xml.Write("\t<result name=\"Naksatra\">\n");

                GregorianDateTime d = new GregorianDateTime();
                d.Set(vc);
                d.TimezoneHours = loc.OffsetUtcHours;
                GregorianDateTime dn;
                GCHourTime dt = new GCHourTime();
                int nak;
                GCEarthData earth = loc.GetEarthData();

                for (int i = 0; i < 30; i++)
                {
                    nak = GCNaksatra.GetNextNaksatra(earth, d, out dn);
                    d.Set(dn);
                    xml.Write("\t\t<day date=\"");
                    xml.Write(d);
                    xml.Write("\">\n");
                    //str.Format("%d.%d.%d", d.day, d.month, d.year);
                    //n = m_list.InsertItem(50, GetNaksatraName(nak));
                    //m_list.SetItemText(n, 1, str);
                    xml.Write("\t\t\t<naksatra id=\"");
                    xml.Write(nak);
                    xml.Write("\" name=\"");
                    xml.Write(GCNaksatra.GetName(nak));
                    xml.Write("\"\n");
                    dt.SetDegTime(d.shour * 360);
                    //time_print(str, dt);
                    xml.Write("\t\t\t\tstarttime=\"");
                    xml.Write(dt);
                    xml.Write("\" />\n");
                    //m_list.SetItemText(n, 2, str);

                    //time_print(str, sun.rise);
                    //m_list.SetItemText(n, 3, str);
                    xml.Write("\t\t\t<sunrise time=\"");
                    xml.Write(GCSunData.CalcSunrise(d, earth));
                    xml.Write("\" />\n");

                    xml.Write("\t\t</day>\n");
                    // increment for non-duplication of naksatra
                    d.Set(dn);
                    d.shour += 1.0 / 8.0;
                }


                xml.Write("\t</result>\n");
                xml.Write("</xml>\n");
            }

            return 1;
        }

        //public static double GetEndHour(GCEarthData earth, GregorianDateTime yesterday, GregorianDateTime today)
        //{
        //    GregorianDateTime nend;
        //    GregorianDateTime snd = new GregorianDateTime();
        //    snd.Set(yesterday);
        //    snd.shour = 0.5;
        //    GCNaksatra.GetNextNaksatra(earth, snd, out nend);
        //    return nend.GetJulian() - today.GetJulian() + nend.shour;
        //}

        private static string[] p_naksatraName = new string[] {
            "Asvini", // GCStrings.Localized("Asvini");
            "Bharani", // GCStrings.Localized("Bharani");
            "Krittika", // GCStrings.Localized("Krittika");
            "Rohini", // GCStrings.Localized("Rohini");
            "Mrigasira", // GCStrings.Localized("Mrigasira");
            "Ardra", // GCStrings.Localized("Ardra");
            "Punarvasu", // GCStrings.Localized("Punarvasu");
            "Pusyami", // GCStrings.Localized("Pusyami");
            "Aslesa", // GCStrings.Localized("Aslesa");
            "Magha", // GCStrings.Localized("Magha");
            "Purva-phalguni", // GCStrings.Localized("Purva-phalguni");
            "Uttara-phalguni", // GCStrings.Localized("Uttara-phalguni");
            "Hasta", // GCStrings.Localized("Hasta");
            "Citra", // GCStrings.Localized("Citra");
            "Swati", // GCStrings.Localized("Swati");
            "Visakha", // GCStrings.Localized("Visakha");
            "Anuradha", // GCStrings.Localized("Anuradha");
            "Jyestha", // GCStrings.Localized("Jyestha");
            "Mula", // GCStrings.Localized("Mula");
            "Purva-asadha", // GCStrings.Localized("Purva-asadha");
            "Uttara-asadha", // GCStrings.Localized("Uttara-asadha");
            "Sravana", // GCStrings.Localized("Sravana");
            "Dhanista", // GCStrings.Localized("Dhanista");
            "Satabhisa", // GCStrings.Localized("Satabhisa");
            "Purva-bhadra", // GCStrings.Localized("Purva-bhadra");
            "Uttara-bhadra", // GCStrings.Localized("Uttara-bhadra");
            "Revati", // GCStrings.Localized("Revati");
        };

        private static string[] p_padaText = new string[] {
            "1st Pada", // GCStrings.Localized("1st Pada");
            "2nd Pada", // GCStrings.Localized("2nd Pada");
            "3rd Pada", // GCStrings.Localized("3rd Pada");
            "4th Pada", // GCStrings.Localized("4th Pada");
        };

        public static string GetPadaText(int i)
        {
            return GCStrings.Localized(p_padaText[i % 4]);
        }

        public static string GetName(int nNaksatra)
        {
            return GCStrings.Localized(p_naksatraName[nNaksatra % 27]);
        }
    }
}
