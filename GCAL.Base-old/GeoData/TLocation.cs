using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GCAL.Base
{
    public class TLocation
    {
        private string p_timezonename = "";
        private TTimeZone p_timezone = null;
        private string p_countrycode = "";
        private TCountry p_country = null;

        public bool Valid { get; set; }
        public int GeonameID { get; set; }
        public string ASCIIName { get; set; }
        public string CityName { get; set; }
        public string AlternativeNames { get; set; }
        public string CountryISOCode 
        {
            get
            {
                return p_countrycode;
            }
            set
            {
                p_countrycode = value;
                p_country = null;
            }
        }
        public TCountry Country
        {
            get
            {
                if (p_country != null) return p_country;
                p_country = TCountry.FindCountryByISOCode(CountryISOCode);
                return p_country ?? TCountry.DefaultCountry;
            }
        }
        public int Population { get; set; }
        public string TimeZoneName { get { return p_timezonename; } set { p_timezonename = value; p_timezone = null; } }
        public TTimeZone TimeZone
        {
            get
            {
                if (p_timezone != null) return p_timezone;
                p_timezone = TTimeZone.FindTimeZoneByName(p_timezonename);
                return p_timezone;
            }
            set
            {
                p_timezone = value;
                p_timezonename = p_timezone.Name;
            }
        }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Elevation { get; set; }

        public TLocation()
        {
            Latitude = 0.0;
            Longitude = 0.0;
            Valid = true;
        }

        public TLocation(string line)
        {
            if (line != null)
            {
                EncodedString = line;
            }
            else
            {
                Valid = false;
            }
        }

        public string EncodedString
        {
            get
            {
                string strTemp = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}",
                    GeonameID, CityName, ASCIIName, AlternativeNames, Latitude, Longitude, "", "", CountryISOCode,
                    "", "", "", "", "", Population, Elevation,
                    "", TimeZoneName, "");
                return strTemp;
            }
            set
            {
                string[] p = value.Split('\t');
                double d;
                int i;
                if (p.Length > 18)
                {
                    GeonameID = int.Parse(p[0]);
                    CityName = p[1];
                    ASCIIName = p[2];
                    AlternativeNames = p[3];
                    if (double.TryParse(p[4], out d)) Latitude = d;
                    if (double.TryParse(p[5], out d)) Longitude = d;
                    // FeatureClass = p[6];
                    // FeatureCode = p[7];
                    CountryISOCode = p[8];
                    // AlternativeCountryCodes = p[9];
                    // Admin1Code = p[10];
                    // Admin2Code = p[11];
                    // Admin3Code = p[12];
                    // Admin4Code = p[13];
                    if (int.TryParse(p[14], out i)) Population = i;
                    if (double.TryParse(p[15], out d)) Elevation = d;
                    // DigitalElevationModel = p[16];
                    TimeZoneName = p[17];
                    // ModificationDate = p[18];
                    Valid = true;
                }
                else
                {
                    Valid = false;
                }
            }
        }

        public void SaveToNode(XmlElement e)
        {
            XH.SetXmlInt(e, "ID", GeonameID);
            XH.SetXmlString(e, "Name", CityName);
            XH.SetXmlString(e, "AS", ASCIIName);
            if (AlternativeNames.Length > 0)
                XH.SetXmlString(e, "Alt", AlternativeNames);
            XH.SetXmlDouble(e, "Y", Latitude);
            XH.SetXmlDouble(e, "X", Longitude);
            if (Elevation > 0.1)
                XH.SetXmlDouble(e, "Z", Elevation);
            XH.SetXmlString(e, "Cnt", CountryISOCode);
            if (Population > 0)
                XH.SetXmlInt(e, "P", Population);
            XH.SetXmlString(e, "TZ", TimeZoneName);
        }

        public void LoadFromNode(XmlElement e)
        {
            GeonameID = XH.GetXmlInt(e, "ID", -1);
            CityName = XH.GetXmlString(e, "Name", "");
            ASCIIName = XH.GetXmlString(e, "AS", "");
            AlternativeNames = XH.GetXmlString(e, "Alt", "");
            Latitude = XH.GetXmlDouble(e, "Y", 0);
            Longitude = XH.GetXmlDouble(e, "X", 0);
            Elevation = XH.GetXmlDouble(e, "Z", 0);
            CountryISOCode = XH.GetXmlString(e, "Cnt", "");
            Population = XH.GetXmlInt(e, "P", 0);
            TimeZoneName = XH.GetXmlString(e, "TZ", "Undefined");
        }

        public GCLocation GetLocationRef()
        {
            GCLocation lc = new GCLocation();
            lc.Title = string.Format("{0} ({1})", CityName, Country.Name);
            lc.Latitude = Latitude;
            lc.Longitude = Longitude;
            lc.TimeZone = TimeZone;
            lc.Country = Country;
            return lc;
        }

        /// <summary>
        /// This is testing if given string is somewhere in the location's texts
        /// </summary>
        /// <param name="subString"></param>
        /// <returns>returns code of field, where substring was found. 
        /// Zero (0) means substring was not found.
        /// 1 : string was found at the start of Name or AlternativeName
        /// 2 : string was found within Name
        /// 3 : string was found within Alternative Name</returns>
        public int ContainsSubstring(string subString)
        {
            if (CityName.StartsWith(subString, StringComparison.CurrentCultureIgnoreCase)
                || (ASCIIName != null && ASCIIName.StartsWith(subString, StringComparison.CurrentCultureIgnoreCase)))
                return 1;
            if (CityName.IndexOf(subString, 0, StringComparison.CurrentCultureIgnoreCase) >= 0
                || (ASCIIName != null && ASCIIName.IndexOf(subString, 0, StringComparison.CurrentCultureIgnoreCase) >= 0))
                return 2;
            int index = AlternativeNames.IndexOf(subString, 0, StringComparison.CurrentCultureIgnoreCase);
            if (index > 0)
                return (AlternativeNames[index - 1] == ',') ? 1 : 3;
            else if (index == 0)
                return 1;
            else
                return 0;
        }

    }
}
