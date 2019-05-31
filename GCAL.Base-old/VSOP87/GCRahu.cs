using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.VSOP87
{
    public class GCRahu
    {
        /// <summary>
        /// Calculates geocentric coordinates for celestial body
        /// </summary>
        /// <param name="julianDate"></param>
        /// <param name="L">Longitude in degrees (0deg to 360deg)</param>
        /// <param name="B">Latitude in degrees (-180deg to 180deg)</param>
        /// <param name="R">Radius in AU</param>
        public static void Calculate(double julianDate, out double L, out double B, out double R)
        {
            double t = (julianDate - 2451545) / 36525;
            double t2 = t*t;

            L = GCMath.putIn360(125.044555 - t*1934.1361849 + t2*0.0020762 + t2*t/467410 - t2*t2/60616000);
            B = 0.0;
            R = 0.3;
        }

    }
}
