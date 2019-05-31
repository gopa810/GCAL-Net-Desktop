using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCRasi
    {
        /*********************************************************************/
        /*                                                                   */
        /* Calculation of Rasi from sun-logitude and ayanamsa                */
        /*                                                                   */
        /*********************************************************************/

        public static int GetRasi(double SunLongitude, double Ayanamsa)
        {
            return GCMath.IntFloor(GCMath.putIn360(SunLongitude - Ayanamsa) / 30.0);
        }

        private static string[] p_rasiName = new string[] {
            "Mesa", // GCStrings.Localized("Mesa");
            "Vrsabha", // GCStrings.Localized("Vrsabha");
            "Mithuna", // GCStrings.Localized("Mithuna");
            "Karka", // GCStrings.Localized("Karka");
            "Simha", // GCStrings.Localized("Simha");
            "Kanya", // GCStrings.Localized("Kanya");
            "Tula", // GCStrings.Localized("Tula");
            "Vrscika", // GCStrings.Localized("Vrscika");
            "Dhanus", // GCStrings.Localized("Dhanus");
            "Makara", // GCStrings.Localized("Makara");
            "Kumbha", // GCStrings.Localized("Kumbha");
            "Mina" // GCStrings.Localized("Mina");
        };

        private static string[] p_rasiNameEn = new string[] {
            "Aries", // GCStrings.Localized("Aries");
            "Taurus", // GCStrings.Localized("Taurus");
            "Gemini", // GCStrings.Localized("Gemini");
            "Cancer", // GCStrings.Localized("Cancer");
            "Leo", // GCStrings.Localized("Leo");
            "Virgo", // GCStrings.Localized("Virgo");
            "Libra", // GCStrings.Localized("Libra");
            "Scorpio", // GCStrings.Localized("Scorpio");
            "Sagittarius", // GCStrings.Localized("Sagittarius");
            "Capricorn", // GCStrings.Localized("Capricorn");
            "Aquarius", // GCStrings.Localized("Aquarius");
            "Pisces", // GCStrings.Localized("Pisces");
        };

        public static string GetName(int i)
        {
            return GCStrings.Localized(p_rasiName[i % 12]);
        }

        public static string GetNameEn(int i)
        {
            return GCStrings.Localized(p_rasiNameEn[i % 12]);
        }
    }
}
