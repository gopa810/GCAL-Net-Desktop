using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base
{
    public class GCCoreAstronomy
    {
        public enum AstronomySystem
        {
            Meeus,
            //SuryaSiddhanta
        }

        static GCCoreAstronomy()
        {
            System = AstronomySystem.Meeus;
            TopocentricPancangam = false;
        }

        public static AstronomySystem System { get; set; }

        public static bool TopocentricPancangam { get; set; }

        private static Dictionary<string, TResultCoreEvents> CoreEventsMap = new Dictionary<string, TResultCoreEvents>();

        public static double GetSunLongitude(GregorianDateTime vct, GCEarthData earth)
        {
            switch(System)
            {
                case AstronomySystem.Meeus:
                    return GCSunData.GetSunLongitude(vct);
            }

            return 0;
        }

        public static GCHourTime CalcSunrise(GregorianDateTime vct, GCEarthData earth)
        {
            return GCSunData.CalcSunrise(vct, earth);
        }

        public static GCHourTime CalcSunset(GregorianDateTime vct, GCEarthData earth)
        {
            return GCSunData.CalcSunset(vct, earth);
        }

        public static double GetMoonLongitude(GregorianDateTime vct, GCEarthData earth)
        {
            switch (System)
            {
                case AstronomySystem.Meeus:
                    GCMoonData moon = new GCMoonData();
                    moon.Calculate(vct.GetJulianComplete());
                    return moon.longitude_deg;
            }

            return 0;
        }

        public static double GetMoonElevation(GCEarthData e, GregorianDateTime vc)
        {
            GCMoonData moon = new GCMoonData();
            double d = vc.GetJulianComplete();
            moon.Calculate(d);
            moon.CorrectEqatorialWithParallax(d, e.latitudeDeg, e.longitudeDeg, 0);
            moon.calc_horizontal(d, e.longitudeDeg, e.latitudeDeg);

            return moon.elevation;
        }

        public static TResultCoreEvents GetCoreEventsYear(GCLocation loc, int year)
        {
            string key = "";

            if (TopocentricPancangam)
                key = string.Format("{0}_{1}_{2}_{3}", loc.GetLongitudeString(), loc.GetLatitudeString(), Convert.ToInt32(loc.Altitude), year);
            else
                key = string.Format("0_0_0_{0}", year);

            // if existing in memory, return it
            if (CoreEventsMap.ContainsKey(key))
                return CoreEventsMap[key];

            string filePath = GCGlobal.GetFileName(GCGlobal.CoreDataFolderPath, key + ".ceb");

            TResultCoreEvents ce = new TResultCoreEvents();

            // if file exists and is correct, return it
            if (File.Exists(filePath))
            {
                if (ce.LoadFile(filePath))
                {
                    CoreEventsMap[key] = ce;
                    return ce;
                }
            }

            ce.Full = true;
            // at last, we have to calculate it
            ce.CalculateEvents(loc, year);
            ce.SaveFile(filePath);
            CoreEventsMap[key] = ce;

            return ce;
        }
    }
}
