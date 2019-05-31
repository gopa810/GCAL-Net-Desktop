using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCYoga
    {
        public static int GetNextYogaStart(GCEarthData ed, GregorianDateTime startDate, out GregorianDateTime nextDate)
        {
	        double phi = 40.0/3.0;
	        double l1, l2, longitudeSun;
	        double jday = startDate.GetJulianComplete();
	        double xj;
            double longitudeMoon;
	        GregorianDateTime d = new GregorianDateTime();
	        d.Set(startDate);
	        GregorianDateTime xd = new GregorianDateTime();
	        double scan_step = 0.5;
	        int prev_tit = 0;
	        int new_tit = -1;
	        double ayanamsha = GCAyanamsha.GetAyanamsa(jday);

            longitudeMoon = GCCoreAstronomy.GetMoonLongitude(d, ed);
            longitudeSun = GCCoreAstronomy.GetSunLongitude(d, ed);
	        l1 = GCMath.putIn360( longitudeMoon + longitudeSun - 2*ayanamsha);
	        prev_tit = Convert.ToInt32(Math.Floor(l1/phi));

	        int counter = 0;
	        while(counter < 20)
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
                longitudeSun = GCCoreAstronomy.GetSunLongitude(d, ed);
		        l2 = GCMath.putIn360( longitudeMoon + longitudeSun - 2*ayanamsha);
		        new_tit = Convert.ToInt32(Math.Floor(l2/phi));

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

        private static string[] p_yogaName = new string[] {
            "Viskumba", // GCStrings.Localized("Viskumba");
            "Priti", // GCStrings.Localized("Priti");
            "Ayusmana", // GCStrings.Localized("Ayusmana");
            "Saubhagya", // GCStrings.Localized("Saubhagya");
            "Sobana", // GCStrings.Localized("Sobana");
            "Atiganda", // GCStrings.Localized("Atiganda");
            "Sukarma", // GCStrings.Localized("Sukarma");
            "Dhriti", // GCStrings.Localized("Dhriti");
            "Sula", // GCStrings.Localized("Sula");
            "Ganda", // GCStrings.Localized("Ganda");
            "Vriddhi", // GCStrings.Localized("Vriddhi");
            "Dhruva", // GCStrings.Localized("Dhruva");
            "Vyagata", // GCStrings.Localized("Vyagata");
            "Harsana", // GCStrings.Localized("Harsana");
            "Vajra", // GCStrings.Localized("Vajra");
            "Siddhi", // GCStrings.Localized("Siddhi");
            "Vyatipata", // GCStrings.Localized("Vyatipata");
            "Variyana", // GCStrings.Localized("Variyana");
            "Parigha", // GCStrings.Localized("Parigha");
            "Siva", // GCStrings.Localized("Siva");
            "Siddha", // GCStrings.Localized("Siddha");
            "Sadhya", // GCStrings.Localized("Sadhya");
            "Subha", // GCStrings.Localized("Subha");
            "Sukla", // GCStrings.Localized("Sukla");
            "Brahma", // GCStrings.Localized("Brahma");
            "Indra", // GCStrings.Localized("Indra");
            "Vaidhriti" // GCStrings.Localized("Vaidhriti");
        };

        public static string GetName(int nYoga)
        {
            return GCStrings.Localized(p_yogaName[nYoga % 27]);
        }
    }
}
