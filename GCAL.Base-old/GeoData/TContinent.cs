using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    /// <summary>
    /// Source data from http://download.geonames.org/export/dump/
    /// </summary>
    public class TContinent
    {
        public string ISOCode { get; set; }
        public string Name { get; set; }

        public static TContinent[] Continents = new TContinent[] {
            new TContinent() { ISOCode = "AF", Name = "Africa" },
            new TContinent() { ISOCode = "AS", Name = "Asia" },
            new TContinent() { ISOCode = "EU", Name = "Europe" },
            new TContinent() { ISOCode = "NA", Name = "North America" },
            new TContinent() { ISOCode = "OC", Name = "Oceania" },
            new TContinent() { ISOCode = "SA", Name = "South America" },
            new TContinent() { ISOCode = "AN", Name = "Antarctica" },
        };

        public static TContinent unknownContinent;

        static TContinent()
        {
            unknownContinent = new TContinent() { ISOCode = "EP", Name = "Earth Planet" };
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", ISOCode, Name);
        }

        /// <summary>
        /// Finds Continent by its code
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        public static TContinent FindByISOCode(string isoCode)
        {
            foreach (TContinent continent in Continents)
            {
                if (continent.ISOCode.Equals(isoCode))
                    return continent;
            }

            return unknownContinent;
        }
    }
}
