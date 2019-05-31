using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using GCAL.Base;
using GCAL.Base.Scripting;

namespace GCAL.Base.VSOP87
{
    public class Testing
    {
        public static void Test()
        {
            StringBuilder sb = new StringBuilder();
            /*
            GregorianDateTime g = new GregorianDateTime(2017, 7, 27);
            g.shour = 5.0;
            g.TimezoneHours = 2.0;

            GCEarthData e = new GCEarthData();
            e.latitudeDeg = 48.0;
            e.longitudeDeg = 17.0;


            SuryaSiddhanta_by_VinayJha ss = new SuryaSiddhanta_by_VinayJha();

            for (int i = 7; i < 31; i++)
            {
                g.day = i;
                SuryaChandraData scd = ss.CalculateSunMoon(g, e);
                int tithi = (int)(scd.dChandra - scd.dSurya) / 12;
                if (tithi < 0) tithi += 30;
                sb.AppendFormat("{0}   {1}\n", g.ToString(), tithi);
            }

            Debugger.Log(0,"", "\n------------ DATA ----------\n");
            Debugger.Log(0,"", sb.ToString());*/

            //e = null;
            //TestScripting1();

            //TestAscendant();

            //TestLocations();

            //TestHouses(sb);
        }

        private static void TestHouses(StringBuilder sb)
        {
            GregorianDateTime g = new GregorianDateTime(2016, 9, 1);
            g.shour = 0.0;
            g.TimezoneHours = 0.0;

            GCEarthData earth = GCGlobal.myLocation.GetEarthData();
            GCEarth e = new GCEarth();
            sb.AppendLine(string.Format("{0,12} {1,8} {2,-15}  ", "Date", "Time", "Julian"));
            sb.AppendLine("---------------------------------------------------------------------------------------");
            for (int i = 0; i < 400; i++)
            {
                double jd = g.GetJulianComplete();
                double t = (jd - 2451545) / 365250;
                double sl1 = GCCoreAstronomy.GetSunLongitude(g, earth);
                double el, eb, er;
                GCEarth.Calculate(jd, out el, out eb, out er);
                double rl, rb, rr;
                GCRahu.Calculate(jd, out rl, out rb, out rr);
                double ml, mb, mr;
                GCMoonData.Calculate(jd, out ml, out mb, out mr);
                double ay = GCAyanamsha.GetAyanamsa(jd);
                sb.AppendLine(string.Format("{0,12} {1,8} {2,-15:F6}", g.ToString(), g.LongTime, jd));
                sb.AppendLine();
                for (int j = 5; j < 6; j++)
                {
                    GCVSOPAstronomy.GetGeocentricCoordinates(j, jd, out rl, out rb, out rr);
                    sb.AppendLine(string.Format("    {0,14} {1:F6}", GCStrings.GetPlanetNameEn(j), rl));
                }
                sb.AppendLine();

                g.AddHours(24);
            }


            File.WriteAllText("d:\\Temp\\gcaltest.txt", sb.ToString());
        }

        private static void TestScripting1()
        {
            GSExecutor es = new GSExecutor();

            GSScript scr = new GSScript();
            es.SetVariable("alg1", new GSNumber() { IntegerValue = 67 });
            es.SetVariable("alg2", new GSString() { Value = "This is string" });
            es.SetVariable("alg3", new GSNumber() { DoubleValue = 18.29893 });

            scr.readTextTemplate("[alg1:08d] [alg2:-18s] [alg3:5.3f]");
            es.ExecuteElement(scr);
            scr.Parts.Clear();
        }

        private static void TestAscendant()
        {
            GCEarthData earth = new GCEarthData();
            earth.latitudeDeg = 48.133;
            earth.longitudeDeg = 17.1;
            GregorianDateTime gt = new GregorianDateTime(2016, 9, 26);
            gt.shour = 0.0;
            gt.TimezoneHours = 2.0;
            for (int i = 0; i < 144; i++)
            {
                double d = earth.GetAscendantDegrees(gt.GetJulianDetailed());
                Debugger.Log(0, "", string.Format("{0} {1}     {2}\n", gt.ToString(), gt.LongTimeString(), d));
                gt.shour += 10 / 1440.0;
            }
        }

        private static void TestLocations()
        {
            foreach (TLocation loc in TLocationDatabase.LocationList)
            {
                TTimeZone tz = loc.TimeZone;
                if (tz == null)
                {
                    Debugger.Log(0, "", string.Format("{0} {1}  => {2}\n", loc.CityName, loc.TimeZoneName, "Undefined"));
                }
                else
                {
                    //Debugger.Log(0, "", string.Format("{0} {1}  => {2}\n", loc.CityName, loc.Country.Name, tz.Name));
                }
            }

            Debugger.Log(0, "", "----\n");

        }
    }
}
