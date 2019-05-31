using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace GCAL.Base
{
    public class GCSankranti
    {
        private static int sankrantiDetermineType = 2;

        public static int GetSankrantiType()
        {
            return sankrantiDetermineType;
        }
        public static int SetSankrantiType(int i)
        {
            int prev = sankrantiDetermineType;
            GCSankranti.sankrantiDetermineType = i;
            return prev;
        }

        private static string[] snam = new string [] {
            "midnight to midnight", // GCStrings.Localized("midnight to midnight");
            "sunrise to sunrise",   // GCStrings.Localized("sunrise to sunrise");
            "noon to noon",         // GCStrings.Localized("noon to noon");
            "sunset to sunset"      // GCStrings.Localized("sunset to sunset");
        };

        public static string GetSankMethodName(int i)
        {
            return GCStrings.Localized(snam[i]);
        }


        /*********************************************************************/
        /*  Finds next time when rasi is changed                             */
        /*                                                                   */
        /*  startDate - starting date and time, timezone member must be valid */
        /*  zodiac [out] - found zodiac sign into which is changed           */
        /*                                                                   */
        /*********************************************************************/

        public static GregorianDateTime GetNextSankranti(GregorianDateTime startDate, GCEarthData earth, out int zodiac)
        {
            GregorianDateTime d = new GregorianDateTime();
            double step = 1.0;
            int count = 0;
            double ld, prev;
            int prev_rasi, new_rasi;
            GregorianDateTime prevday;

            d.Set(startDate);
            //d.ChangeTimeZone(0.0);
            //d.shour = 0.0;
            zodiac = 0;

            prev = GCMath.putIn360(GCCoreAstronomy.GetSunLongitude(d, earth) - GCAyanamsha.GetAyanamsa(d.GetJulian()));
            prev_rasi = GCMath.IntFloor(prev / 30.0);

            while (count < 20)
            {
                prevday = new GregorianDateTime();
                prevday.Set(d);
                d.shour += step;
                if (d.shour > 1.0)
                {
                    d.shour -= 1.0;
                    d.NextDay();
                }

                ld = GCMath.putIn360(GCCoreAstronomy.GetSunLongitude(d, earth) - GCAyanamsha.GetAyanamsa(d.GetJulian()));
                new_rasi = GCMath.IntFloor(ld / 30.0);

                if (prev_rasi != new_rasi)
                {
                    zodiac = new_rasi;
                    //v uplynulom dni je sankranti
                    step *= 0.5;
                    d.Set(prevday);
                    count++;
                    continue;
                }
            }

            return d;
        }

        public static XmlDocument GetSankrantiListXml(GCLocation loc, GregorianDateTime vcStart, GregorianDateTime vcEnd)
        {
            GregorianDateTime d = new GregorianDateTime();
            int zodiac;

            XmlDocument doc = new XmlDocument();
            XmlElement e1 = doc.CreateElement("xml");
            doc.AppendChild(e1);
            XmlElement e2, e3;
            GCEarthData earth = loc.GetEarthData();

            // open file
            d.Set(vcStart);

            e2 = doc.CreateElement("request");
            e1.AppendChild(e2);
            e2.SetAttribute("name", "Sankranti");
            e2.SetAttribute("version", GCStrings.getString(130));

            e2.SetAttribute("longitude", loc.Longitude.ToString());
            e2.SetAttribute("latitude", loc.Latitude.ToString());
            e2.SetAttribute("timezone", loc.OffsetUtcHours.ToString());
            e2.SetAttribute("startdate", vcStart.ToString());
            e2.SetAttribute("enddate", vcEnd.ToString());

            e2 = doc.CreateElement("result");
            e2.SetAttribute("name", "SankrantiList");
            e1.AppendChild(e2);

            while (d.IsBeforeThis(vcEnd))
            {
                d.Set(GCSankranti.GetNextSankranti(d, earth, out zodiac));
                d.InitWeekDay();

                e3 = doc.CreateElement("sank");
                e2.AppendChild(e3);

                e3.SetAttribute("date", d.ToString());
                e3.SetAttribute("dayweekid", d.dayOfWeek.ToString());
                e3.SetAttribute("dayweek", GCCalendar.GetWeekdayName(d.dayOfWeek));
                e3.SetAttribute("time", d.LongTimeString());
                e3.SetAttribute("rasi", zodiac.ToString());
                e3.SetAttribute("rasiName", GCRasi.GetName(zodiac));
                e3.SetAttribute("rasiNameEn", GCRasi.GetNameEn(zodiac));

                d.NextDay();
                d.NextDay();
            }

            return doc;
        }

    }
}
