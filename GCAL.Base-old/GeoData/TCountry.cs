using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;

namespace GCAL.Base
{
    public class TCountry
    {
        public static bool Modified = false;
        public static List<TCountry> Countries = new List<TCountry>();

        public int FirstDayOfWeek { get; set; }
        public string ISOCode { get; set; }
        public string ISO3Code { get; set; }
        public string Fips { get; set; }
        public string Name { get; set; }
        public string Capital { get; set; }
        public double Area { get; set; }
        public double Population { get; set; }
        public string ContinentISOCode { get; set; }
        public TContinent Continent
        {
            get
            {
                return TContinent.FindByISOCode(ContinentISOCode);
            }
        }
        public string Neighbours { get; set; }


        public static TCountry DefaultCountry = new TCountry() { ISOCode = "UC", ISO3Code = "UCO", Name = "(Unknown Country)",
        Population = 1000000, Area = 189230.0, Capital = "(Uknown City)", ContinentISOCode = "EP", Fips = "", Neighbours = ""};


        public TCountry()
        {
            FirstDayOfWeek = -1;
        }

        public override string ToString()
        {
            return Name;
        }

        public static TCountry GetCurrentCountry()
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            RegionInfo ri = new RegionInfo(ci.LCID);
            TCountry tc = FindCountryByISOCode(ri.TwoLetterISORegionName);
            if (tc == null)
                return FindCountryByISOCode("ID");
            else
                return tc;
        }

        public static TCountry FindCountryByISOCode(string s)
        {
            foreach (TCountry tc in Countries)
            {
                if (tc.ISOCode.Equals(s))
                    return tc;
            }
            return null;
        }
        
        public static TCountry FindCountryByName(string s)
        {
            foreach (TCountry tc in Countries)
            {
                if (tc.Name.Equals(s))
                    return tc;
            }
            return null;
        }

        /// <summary>
        /// Loading countries from permanent storage
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public static int LoadFile(string strFile)
        {
	        if (!File.Exists(strFile))
	        {
                File.WriteAllText(strFile, Properties.Resources.countries2016);
	        }

            XmlDocument doc = new XmlDocument();
            doc.Load(strFile);

            foreach(XmlElement ec in doc.GetElementsByTagName("Country"))
            {
                TCountry tc = new TCountry();
                tc.LoadFromNode(ec);
                Countries.Add(tc);
            }

	        return Countries.Count();
        }

        public static int SaveFile(string szFile)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement e1 = doc.CreateElement("Countries");
            doc.AppendChild(e1);

            foreach(TCountry tc in Countries)
            {
                XmlElement e2 = doc.CreateElement("Country");
                e1.AppendChild(e2);
                tc.SaveToNode(e2);
            }

            doc.Save(szFile);

            return 1;
        }

        public static int SetCountryName(int nSelected, string psz)
        {
	        Countries[nSelected].Name = psz;
	        Modified = true;
	        return 1;
        }


        public static bool IsModified()
        {
	        return Modified;
        }

        public void SaveToNode(XmlElement e)
        {
            e.SetAttribute("ISO", ISOCode);
            e.SetAttribute("ISO3", ISO3Code);
            e.SetAttribute("Fips", Fips);
            e.SetAttribute("Name", Name);
            e.SetAttribute("Capital", Capital);
            e.SetAttribute("Area", Area.ToString());
            e.SetAttribute("Population", Population.ToString());
            e.SetAttribute("ContinentISOCode", ContinentISOCode);
            e.SetAttribute("Neighbours", Neighbours);
            e.SetAttribute("FirstDayOfWeek", FirstDayOfWeek.ToString());
        }

        public void LoadFromNode(XmlElement e)
        {
            if (e.HasAttribute("ISO"))
                ISOCode = e.GetAttribute("ISO");
            if (e.HasAttribute("ISO3"))
                ISO3Code = e.GetAttribute("ISO3");
            if (e.HasAttribute("Fips"))
                Fips = e.GetAttribute("Fips");
            if (e.HasAttribute("Name"))
                Name = e.GetAttribute("Name");
            if (e.HasAttribute("Capital"))
                Capital = e.GetAttribute("Capital");
            if (e.HasAttribute("Area"))
            {
                double d;
                if (double.TryParse(e.GetAttribute("Area"), out d))
                    Area = d;
            }
            if (e.HasAttribute("Population"))
            {
                double d;
                if (double.TryParse(e.GetAttribute("Population"), out d))
                    Population = d;
            }
            if (e.HasAttribute("ContinentISOCode"))
                ContinentISOCode = e.GetAttribute("ContinentISOCode");
            if (e.HasAttribute("Neighbours"))
                Neighbours = e.GetAttribute("Neighbours");
            if (e.HasAttribute("FirstDayOfWeek"))
            {
                int d;
                if (int.TryParse(e.GetAttribute("FirstDayOfWeek"), out d))
                    FirstDayOfWeek = d;
            }
        }

        public string EncodedString
        {
            get
            {
                return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}",
                        ISOCode, ISO3Code, Fips,
                        Name, Capital, Area, Population,
                        ContinentISOCode, Neighbours,
                        FirstDayOfWeek);
            }
            set
            {
                string[] p = value.Split('\t');
                if (p.Length >= 9)
                {
                    TCountry tc = this;
                    tc.ISOCode = p[0];
                    tc.ISO3Code = p[1];
                    tc.Fips = p[2];
                    tc.Name = p[3];
                    tc.Capital = p[4];
                    tc.Area = double.Parse(p[5]);
                    tc.Population = double.Parse(p[6]);
                    tc.ContinentISOCode = p[7];
                    tc.Neighbours = p[8];
                    tc.FirstDayOfWeek = int.Parse(p[9]);
                }
            }
        }

    }
}
