using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class TResultApp: TResultBase
    {
        public static readonly int TRESULT_APP_CELEBS = 3;
        public GCLocation Location;
        public GregorianDateTime eventTime;
        public GCAstroData details;
        public bool b_adhika;
        public int[] celeb_gy;
        public GregorianDateTime[] celeb_date;

        public class AppDayBase : GSCore
        {
            public int DsCondition = -1;
            public AppDayBase()
            {
            }

            public AppDayBase(int cond)
            {
                DsCondition = cond;
            }

            public override GSCore GetPropertyValue(string s)
            {
                if (s.Equals("DsCondition"))
                    return new GSNumber(DsCondition);
                return base.GetPropertyValue(s);
            }
        }

        public class AppDaySeparator : AppDayBase
        {
            public string Name = "";
            public AppDaySeparator()
            {
            }

            public AppDaySeparator(string n)
            {
                Name = n;
            }

            public AppDaySeparator(int i, string n)
            {
                DsCondition = i;
                Name = n;
            }

            public override GSCore GetPropertyValue(string s)
            {
                if (s.Equals("isInfo"))
                    return new GSBoolean(false);
                if (s.Equals("isSeparator"))
                    return new GSBoolean(true);
                if (s.Equals("Name"))
                    return new GSString(Name);
                return base.GetPropertyValue(s);
            }
        }

        public class AppDayInfo: AppDayBase
        {
            public string Name = "";
            public string Value = "";

            public AppDayInfo()
            {
            }

            public AppDayInfo(string a, string b)
            {
                Name = a;
                Value = b;
            }

            public AppDayInfo(int cond, string a, string b)
            {
                DsCondition = cond;
                Name = a;
                Value = b;
            }

            public override GSCore GetPropertyValue(string s)
            {
                if (s.Equals("isSeparator"))
                    return new GSBoolean(false);
                if (s.Equals("isInfo"))
                    return new GSBoolean(true);
                if (s.Equals("Name"))
                    return new GSString(Name);
                if (s.Equals("Value"))
                    return new GSString(Value);
                return base.GetPropertyValue(s);
            }

        }

        public override GSCore GetPropertyValue(string s)
        {
            if (s.Equals("items"))
            {
                GSList list = new GSList();
                foreach (AppDayBase adb in MainInfo)
                    list.Add(adb);
                return list;
            }
            else if (s.Equals("location"))
            {
                return Location;
            }
            else if (s.Equals("eventTime"))
            {
                return eventTime;
            }
            else if (s.Equals("astroData"))
            {
                return details;
            }
            return base.GetPropertyValue(s);
        }

        public List<AppDayBase> MainInfo = new List<AppDayBase>();

        public void calculateAppDay(GCLocation location, GregorianDateTime eventDate)
        {
            //MOONDATA moon;
            //SUNDATA sun;
            GCAstroData d = this.details = new GCAstroData();
            GregorianDateTime vc = new GregorianDateTime();
            GregorianDateTime vcsun = new GregorianDateTime();
            GCEarthData m_earth = location.GetEarthData();

            vc.Set(eventDate);
            vcsun.Set(eventDate);

            this.b_adhika = false;
            this.eventTime = new GregorianDateTime(eventDate);
            Location = location;

            //d.nTithi = GetPrevTithiStart(m_earth, vc, dprev);
            //GetNextTithiStart(m_earth, vc, dnext);
            vcsun.shour -= vcsun.TimezoneHours / 24.0;
            vcsun.NormalizeValues();
            vcsun.TimezoneHours = 0.0;
            d.sunRise = new GCHourTime();
            d.sunRise.TotalDays = vc.shour;
            d.sunRise.longitude = GCCoreAstronomy.GetSunLongitude(vcsun, m_earth);
            d.sunRise.longitudeMoon = GCCoreAstronomy.GetMoonLongitude(vcsun, m_earth);
            d.Ayanamsa = GCAyanamsha.GetAyanamsa(vc.GetJulianComplete());
            d.sunRise.Ayanamsa = d.Ayanamsa;

            // tithi


            d.Masa = d.MasaCalc(vc, m_earth);
            if (d.Masa == (int)MasaId.ADHIKA_MASA)
            {
                d.Masa = d.sunRise.RasiOfSun;
                this.b_adhika = true;
            }

            vc.Today();
            vc.TimezoneHours = m_earth.OffsetUtcHours;
            int m = 0;
            GaurabdaDate va = new GaurabdaDate();
            GregorianDateTime vctemp;

            va.tithi = d.sunRise.Tithi;
            va.masa = d.Masa;
            va.gyear = GCCalendar.GetGaurabdaYear(vc, m_earth);
            if (va.gyear < d.GaurabdaYear)
                va.gyear = d.GaurabdaYear;

            MainInfo.Add(new AppDayInfo(GCStrings.getString(7), eventDate.ToString()));
            MainInfo.Add(new AppDayBase());
            MainInfo.Add(new AppDayInfo(GCStrings.getString(8), eventDate.ShortTimeString()));
            MainInfo.Add(new AppDayBase());
            MainInfo.Add(new AppDayBase());
            MainInfo.Add(new AppDayInfo(GCStrings.getString(9), location.Title));
            MainInfo.Add(new AppDayInfo(GCStrings.getString(10), location.GetLatitudeString()));
            MainInfo.Add(new AppDayInfo(GCStrings.getString(11), location.GetLongitudeString()));
            MainInfo.Add(new AppDayInfo(GCStrings.Localized("Timezone"), location.TimeZoneName));
            MainInfo.Add(new AppDayInfo("DST", "N/A"));
            MainInfo.Add(new AppDayBase());
            MainInfo.Add(new AppDayInfo(GCStrings.getString(13), GCTithi.GetName(d.sunRise.Tithi)));
            MainInfo.Add(new AppDayInfo(GCStrings.getString(14), string.Format("{0:00.000}%", d.sunRise.TithiElapse)));
            MainInfo.Add(new AppDayInfo(GCStrings.getString(15), GCNaksatra.GetName(d.sunRise.Naksatra)));
            MainInfo.Add(new AppDayInfo(GCStrings.getString(16), string.Format("{0:00.000}% ({1} pada)", d.sunRise.NaksatraElapse, GCStrings.getString(811 + d.sunRise.NaksatraPada))));
            MainInfo.Add(new AppDayInfo(GCStrings.Localized("Moon Rasi"), GCRasi.GetName(d.sunRise.RasiOfMoon)));
            MainInfo.Add(new AppDayInfo(GCStrings.Localized("Sun Rasi"), GCRasi.GetName(d.sunRise.RasiOfSun)));
            MainInfo.Add(new AppDayInfo(GCStrings.getString(20), GCPaksa.GetName(d.sunRise.Paksa)));

            if (b_adhika == true)
            {
                MainInfo.Add(new AppDayInfo(GCStrings.getString(22), string.Format("{0} {1}", GCMasa.GetName(d.Masa), GCStrings.getString(21))));
            }
            else
                MainInfo.Add(new AppDayInfo(GCStrings.getString(22), GCMasa.GetName(d.Masa)));
            MainInfo.Add(new AppDayInfo(GCStrings.getString(23), d.GaurabdaYear.ToString()));

            if (GCDisplaySettings.Current.getValue(48) == 1)
            {
                MainInfo.Add(new AppDayBase(GCDS.APP_CHILDNAMES));
                MainInfo.Add(new AppDaySeparator(GCStrings.getString(17)));
                MainInfo.Add(new AppDayBase(GCDS.APP_CHILDNAMES));

                MainInfo.Add(new AppDayInfo(GCDS.APP_CHILDNAMES, GCStrings.getString(18), GCStrings.GetNaksatraChildSylable(d.sunRise.Naksatra, d.sunRise.NaksatraPada) + "..."));
                MainInfo.Add(new AppDayInfo(GCDS.APP_CHILDNAMES, GCStrings.getString(19), GCStrings.GetRasiChildSylable(d.sunRise.RasiOfMoon) + "..."));
            }

            MainInfo.Add(new AppDayBase());
            MainInfo.Add(new AppDaySeparator(GCStrings.getString(24)));
            MainInfo.Add(new AppDayBase());


            celeb_date = new GregorianDateTime[TRESULT_APP_CELEBS];
            celeb_gy = new int[TRESULT_APP_CELEBS];

            for (int i = 0; i < TRESULT_APP_CELEBS + 3; i++)
            {
                GCCalendar.VATIMEtoVCTIME(va, out vctemp, m_earth);
                if (va.gyear > d.GaurabdaYear)
                {
                    if (m < TRESULT_APP_CELEBS)
                    {
                        MainInfo.Add(new AppDayInfo(string.Format("Gaurabda {0}", va.gyear), vctemp.ToString()));
                        this.celeb_date[m] = new GregorianDateTime(vctemp);
                        this.celeb_gy[m] = va.gyear;
                        m++;
                    }
                }
                va.gyear++;
            }
        }

        public override string formatText(string df)
        {
            GSScript script = new GSScript();
            switch (df)
            {
                case GCDataFormat.PlainText:
                    script.readTextTemplate(Properties.Resources.TplAppDayPlain);
                    break;
                case GCDataFormat.Rtf:
                    script.readTextTemplate(Properties.Resources.TplAppDayRtf);
                    break;
                case GCDataFormat.HTML:
                    script.readTextTemplate(Properties.Resources.TplAppDayHtml);
                    break;
                case GCDataFormat.XML:
                    script.readTextTemplate(Properties.Resources.TplAppDayXml);
                    break;
                case GCDataFormat.CSV:
                    script.readTextTemplate(Properties.Resources.TplAppDayCsv);
                    break;
                default:
                    break;
            }


            GSExecutor engine = new GSExecutor();
            engine.SetVariable("appday", this);
            engine.SetVariable("location", this.Location);
            engine.SetVariable("app", GCUserInterface.Shared);
            engine.ExecuteElement(script);


            return engine.getOutput();
        }

        public override TResultFormatCollection getFormats()
        {
            TResultFormatCollection coll = base.getFormats();

            coll.ResultName = "AppearanceDay";
            coll.Formats.Add(new TResultFormat("Text File", "txt", GCDataFormat.PlainText));
            coll.Formats.Add(new TResultFormat("Rich Text File", "rtf", GCDataFormat.Rtf));
            coll.Formats.Add(new TResultFormat("XML File", "xml", GCDataFormat.XML));
            coll.Formats.Add(new TResultFormat("Comma Separated Values", "csv", GCDataFormat.CSV));
            coll.Formats.Add(new TResultFormat("HTML File (in List format)", "htm", GCDataFormat.HTML));
            return coll;
        }

    }
}
