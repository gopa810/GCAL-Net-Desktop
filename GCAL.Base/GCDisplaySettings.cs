using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;

namespace GCAL.Base
{
    public class CShowSetting
    {
        public int val;
        public int old_val;
        public string text;

        public CShowSetting(int a, int b, string t)
        {
            val = a;
            old_val = b;
            text = t;
        }
    }

    public class GCDisplaySettings
    {
        public string Name { get; set; }

        public CShowSetting[] gss = null;

        public GCDisplaySettings(string name)
        {
            Name = name;
            Reset();
        }

        public GCDisplaySettings(GCDisplaySettings ds)
        {
            Name = ds.Name + " (copy)";
            Reset();
            for(int i = 0; i < gss.Length; i++)
            {
                gss[i] = ds.gss[i];
            }
        }

        public int getCount()
        {
            int i = 0;
            while (gss[i].text != null)
                i++;
            return i;
        }

        public int getCountChanged()
        {
            int i, count = 0;
            int size = getCount();

            for (i = 0; i < size; i++)
            {
                if (gss[i].val != gss[i].old_val)
                    count++;
                gss[i].old_val = gss[i].val;
            }

            return count;
        }

        public string getSettingName(int i)
        {
            return gss[i].text;
        }

        public int getValue(int i)
        {
            switch (i)
            {
                case 0: return Tithiatarunodaya ? 1 : 0;
                case 1: return ArunodayaTime ? 1 : 0;
                case 2: return SunriseTime ? 1 : 0;
                case 3: return SunsetTime ? 1 : 0;
                case 4: return MoonriseTime ? 1 : 0;
                case 5: return MoonsetTime ? 1 : 0;
                case 6: return Festivals ? 1 : 0;
                case 7: return Infoaboutksayatithi ? 1 : 0;
                case 8: return Infoaboutvriddhitithi ? 1 : 0;
                case 9: return SunLongitude ? 1 : 0;
                case 10: return MoonLongitude ? 1 : 0;
                case 11: return Ayanamshavalue ? 1 : 0;
                case 12: return JulianDay ? 1 : 0;
                case 13: return CaturmasyaPurnimaSystem ? 1 : 0;
                case 14: return CaturmasyaPratipatSystem ? 1 : 0;
                case 15: return CaturmasyaEkadasiSystem ? 1 : 0;
                case 16: return SankrantiInfo ? 1 : 0;
                case 17: return EkadasiInfo ? 1 : 0;
                case 18: return CalHeaderType;
                case 20: return Donotshowemptydays ? 1 : 0;
                case 21: return Showbeginingofmasa ? 1 : 0;
                case 22: return AppearancedaysoftheLord ? 1 : 0;
                case 23: return EventsinthepastimesoftheLord ? 1 : 0;
                case 24: return AppDisappofRecentAcaryas ? 1 : 0;
                case 25: return AppDisappofMahaprabhusAssociatesandOtherAcaryas ? 1 : 0;
                case 26: return ISKCONsHistoricalEvents ? 1 : 0;
                case 27: return BengalspecificHolidays ? 1 : 0;
                case 28: return MyPersonalEvents ? 1 : 0;
                case 29: return TodaySunrise ? 1 : 0;
                case 30: return TodayNoon ? 1 : 0;
                case 31: return TodaySunset ? 1 : 0;
                case 32: return SandhyaTimes ? 1 : 0;
                case 33: return SunriseInfo ? 1 : 0;
                case 34: return NoonTime ? 1 : 0;
                case 35: return NoticeaboutDST ? 1 : 0;
                case 36: return CalColNaksatra ? 1 : 0;
                case 37: return CalColYoga ? 1 : 0;
                case 38: return CalColFast ? 1 : 0;
                case 39: return CalColPaksa ? 1 : 0;
                case 40: return FirstDayinWeek ? 1 : 0;
                case 41: return CalColMoonRasi ? 1 : 0;
                case 42: return OldStyleFastingtext ? 1 : 0;
                case 43: return NameofmonthType ? 1 : 0;
                case 44: return EditableDefaultEvents ? 1 : 0;
                case 45: return TodayBrahmaMuhurta ? 1 : 0;
                case 46: return TodayRasioftheMoon ? 1 : 0;
                case 47: return TodayNaksatraPadadetails ? 1 : 0;
                case 48: return ChildNamesSuggestions ? 1 : 0;
                case 49: return MasaNameFormat ? 1 : 0;
                case 50: return EkadasiParanadetails ? 1 : 0;
                case 51: return Aniversaryshowformat ? 1 : 0;
                case 52: return Sunevents ? 1 : 0;
                case 53: return Tithievents ? 1 : 0;
                case 54: return NaksatraEvents ? 1 : 0;
                case 55: return SankrantiEvents ? 1 : 0;
                case 56: return ConjunctionEvents ? 1 : 0;
                case 57: return Rahukalam ? 1 : 0;
                case 58: return Yamaghanti ? 1 : 0;
                case 59: return Gulikalam ? 1 : 0;
                case 60: return Moonevents ? 1 : 0;
                case 61: return Moonrasi ? 1 : 0;
                case 62: return Ascendent ? 1 : 0;
                case 63: return Sortresultscoreevents ? 1 : 0;
                case 64: return AbhijitMuhurta ? 1 : 0;
                case 65: return YogaEvents ? 1 : 0;
                case 66: return CaturmasyaSystem;
                case 67: return CalColCoreEvents ? 1 : 0;
                case 68: return CalColSunrise ? 1 : 0;
                case 69: return CalColNoon ? 1 : 0;
                case 70: return CalColSunset ? 1 : 0;
                default: return gss[i].val;
            }
        }

        public void setValue(int i, int val)
        {
            bool bval = val == 0 ? false : true;
            switch(i)
            {
                case 0: Tithiatarunodaya = bval; break;
                case 1: ArunodayaTime = bval; break;
                case 2: SunriseTime = bval; break;
                case 3: SunsetTime = bval; break;
                case 4: MoonriseTime = bval; break;
                case 5: MoonsetTime = bval; break;
                case 6: Festivals = bval; break;
                case 7: Infoaboutksayatithi = bval; break;
                case 8: Infoaboutvriddhitithi = bval; break;
                case 9: SunLongitude = bval; break;
                case 10: MoonLongitude = bval; break;
                case 11: Ayanamshavalue = bval; break;
                case 12: JulianDay = bval; break;
                case 13: CaturmasyaPurnimaSystem = bval; break;
                case 14: CaturmasyaPratipatSystem = bval; break;
                case 15: CaturmasyaEkadasiSystem = bval; break;
                case 16: SankrantiInfo = bval; break;
                case 17: EkadasiInfo = bval; break;
                case 18: CalHeaderType = val; break;
                case 20: Donotshowemptydays = bval; break;
                case 21: Showbeginingofmasa = bval; break;
                case 22: AppearancedaysoftheLord = bval; break;
                case 23: EventsinthepastimesoftheLord = bval; break;
                case 24: AppDisappofRecentAcaryas = bval; break;
                case 25: AppDisappofMahaprabhusAssociatesandOtherAcaryas = bval; break;
                case 26: ISKCONsHistoricalEvents = bval; break;
                case 27: BengalspecificHolidays = bval; break;
                case 28: MyPersonalEvents = bval; break;
                case 29: TodaySunrise = bval; break;
                case 30: TodayNoon = bval; break;
                case 31: TodaySunset = bval; break;
                case 32: SandhyaTimes = bval; break;
                case 33: SunriseInfo = bval; break;
                case 34: NoonTime = bval; break;
                case 35: NoticeaboutDST = bval; break;
                case 36: CalColNaksatra = bval; break;
                case 37: CalColYoga = bval; break;
                case 38: CalColFast = bval; break;
                case 39: CalColPaksa = bval; break;
                case 40: FirstDayinWeek = bval; break;
                case 41: CalColMoonRasi = bval; break;
                case 42: OldStyleFastingtext = bval; break;
                case 43: NameofmonthType = bval; break;
                case 44: EditableDefaultEvents = bval; break;
                case 45: TodayBrahmaMuhurta = bval; break;
                case 46: TodayRasioftheMoon = bval; break;
                case 47: TodayNaksatraPadadetails = bval; break;
                case 48: ChildNamesSuggestions = bval; break;
                case 49: MasaNameFormat = bval; break;
                case 50: EkadasiParanadetails = bval; break;
                case 51: Aniversaryshowformat = bval; break;
                case 52: Sunevents = bval; break;
                case 53: Tithievents = bval; break;
                case 54: NaksatraEvents = bval; break;
                case 55: SankrantiEvents = bval; break;
                case 56: ConjunctionEvents = bval; break;
                case 57: Rahukalam = bval; break;
                case 58: Yamaghanti = bval; break;
                case 59: Gulikalam = bval; break;
                case 60: Moonevents = bval; break;
                case 61: Moonrasi = bval; break;
                case 62: Ascendent = bval; break;
                case 63: Sortresultscoreevents = bval; break;
                case 64: AbhijitMuhurta = bval; break;
                case 65: YogaEvents = bval; break;
                case 66: CaturmasyaSystem = val; break;
                case 67: CalColCoreEvents = bval; break;
                case 68: CalColSunrise = bval; break;
                case 69: CalColNoon = bval; break;
                case 70: CalColSunset = bval; break;
                default:
                    gss[i].val = val;
                    gss[i].old_val = val;
                    break;
            }
        }

        public bool Tithiatarunodaya = false;
        public bool ArunodayaTime = false;
        public bool SunriseTime = false;
        public bool SunsetTime = false;
        public bool MoonriseTime = false;
        public bool MoonsetTime = false;
        public bool Festivals = true;
        public bool Infoaboutksayatithi = false;
        public bool Infoaboutvriddhitithi = false;
        public bool SunLongitude = false;
        public bool MoonLongitude = false;
        public bool Ayanamshavalue = false;
        public bool JulianDay = false;
        public bool CaturmasyaPurnimaSystem = false;
        public bool CaturmasyaPratipatSystem = true;
        public bool CaturmasyaEkadasiSystem = false;
        public bool SankrantiInfo = true;
        public bool EkadasiInfo = true;
        public int CalHeaderType = 1;
        public bool Donotshowemptydays = false;
        public bool Showbeginingofmasa = false;
        public bool AppearancedaysoftheLord = true;
        public bool EventsinthepastimesoftheLord = true;
        public bool AppDisappofRecentAcaryas = true;
        public bool AppDisappofMahaprabhusAssociatesandOtherAcaryas = true;
        public bool ISKCONsHistoricalEvents = true;
        public bool BengalspecificHolidays = true;
        public bool MyPersonalEvents = true;
        public bool TodaySunrise = true;
        public bool TodayNoon = true;
        public bool TodaySunset = true;
        public bool SandhyaTimes = false;
        public bool SunriseInfo = true;
        public bool NoonTime = false;
        public bool NoticeaboutDST = true;
        public bool CalColNaksatra = true;
        public bool CalColYoga = true;
        public bool CalColFast = true;
        public bool CalColPaksa = true;
        public bool FirstDayinWeek = false;
        public bool CalColMoonRasi = false;
        public bool OldStyleFastingtext = false;
        public bool NameofmonthType = false;
        public bool EditableDefaultEvents = false;
        public bool TodayBrahmaMuhurta = false;
        public bool TodayRasioftheMoon = false;
        public bool TodayNaksatraPadadetails = false;
        public bool ChildNamesSuggestions = false;
        public bool MasaNameFormat = false;
        public bool EkadasiParanadetails = false;
        public bool Aniversaryshowformat = false;
        public bool Sunevents = true;
        public bool Tithievents = true;
        public bool NaksatraEvents = true;
        public bool SankrantiEvents = true;
        public bool ConjunctionEvents = true;
        public bool Rahukalam = false;
        public bool Yamaghanti = false;
        public bool Gulikalam = false;
        public bool Moonevents = false;
        public bool Moonrasi = false;
        public bool Ascendent = false;
        public bool Sortresultscoreevents = true;
        public bool AbhijitMuhurta = false;
        public bool YogaEvents = false;
        public int CaturmasyaSystem = 1;
        public bool CalColCoreEvents = false;
        public bool CalColSunrise = false;
        public bool CalColNoon = false;
        public bool CalColSunset = false;

        public bool AdvFestivalFirstDay = false;


        public void setBoolValue(int i, bool val)
        {
            setValue(i, val ? 1 : 0);
        }

        public void Reset()
        {
            gss = new CShowSetting[]{
                new CShowSetting(0, 0, "Tithi at arunodaya"),//0
	            new CShowSetting(0, 0, "Arunodaya Time"),//1
	            new CShowSetting(0, 0, "Sunrise Time"),//2
	            new CShowSetting(0, 0, "Sunset Time"),//3
	            new CShowSetting(0, 0, "Moonrise Time"),//4
	            new CShowSetting(0, 0, "Moonset Time"),//5
	            new CShowSetting(1, 1, "Festivals"),//6
	            new CShowSetting(0, 0, "Info about ksaya tithi"),//7
	            new CShowSetting(0, 0, "Info about vriddhi tithi"),//8
	            new CShowSetting(0, 0, "Sun Longitude"),//9
	            new CShowSetting(0, 0, "Moon Longitude"),//10
	            new CShowSetting(0, 0, "Ayanamsha value"),//11
	            new CShowSetting(0, 0, "Julian Day"),//12
	            new CShowSetting(0, 0, "Caturmasya Purnima System"), //13
	            new CShowSetting(1, 1, "Caturmasya Pratipat System"), //14
	            new CShowSetting(0, 0, "Caturmasya Ekadasi System"), //15
	            new CShowSetting(1, 1, "Sankranti Info"), //16
	            new CShowSetting(1, 1, "Ekadasi Info"), //17
	            new CShowSetting(1, 1, "Masa Header Info"), //18
	            new CShowSetting(0, 0, "Month Header Info"), //19
	            new CShowSetting(0, 0, "Do not show empty days"), //20
	            new CShowSetting(0, 0, "Show begining of masa"), //21
	            new CShowSetting(1, 1, "Appearance days of the Lord"),//22
	            new CShowSetting(1, 1, "Events in the pastimes of the Lord"),//23
	            new CShowSetting(1, 1, "App, Disapp of Recent Acaryas"),//24
	            new CShowSetting(1, 1, "App, Disapp of Mahaprabhu's Associates and Other Acaryas"),//25
	            new CShowSetting(1, 1, "ISKCON's Historical Events"),//26
	            new CShowSetting(1, 1, "Bengal-specific Holidays"),//27
	            new CShowSetting(1, 1, "My Personal Events"), //28
	            new CShowSetting(1, 1, "Todat Sunrise"),  //29 Today sunrise
	            new CShowSetting(1, 1, "Today Noon"),  //30 today noon
	            new CShowSetting(1, 1, "Today Sunset"),  //31 today sunset
	            new CShowSetting(0, 0, "Sandhya Times"),  //32 today + sandhya times
	            new CShowSetting(1, 1, "Sunrise Info"),  //33 today sunrise info
	            new CShowSetting(0, 0, "Noon Time"),  //34 astro - noon time
	            new CShowSetting(1, 1, "Notice about DST"), //35 notice about the change of the DST
	            new CShowSetting(1, 1, "Naksatra"), // 36 naksatra info for each day
	            new CShowSetting(1, 1, "Yoga"), //37 yoga info for each day
	            new CShowSetting(1, 1, "Fasting Flag"),//38
	            new CShowSetting(1, 1, "Paksa Info"),//39 paksa info
	            new CShowSetting(0, 0, "First Day in Week"),//40 first day in week
	            new CShowSetting(0, 0, "Rasi"), //41 moon rasi for each calendar day
	            new CShowSetting(0, 0, "Old Style Fasting text"), //42 old style fasting text
	            new CShowSetting(0, 0, "Name of month - type"), //43 month type name 0-vaisnava,1-bengal,2-hindu,3-vedic
	            new CShowSetting(0, 0, "Editable Default Events"), //44 editable default events
	            new CShowSetting(0, 0, "Today Brahma Muhurta"),     //45 brahma muhurta in today screen
	            new CShowSetting(0, 0, "Today Rasi of the Moon"), // 46 rasi of the moon in today screen
	            new CShowSetting(0, 0, "Today Naksatra Pada details"), // 47 naksatra pada details in today screen
	            new CShowSetting(0, 0, "Child Names Suggestions"), // 48 child name suggestions in Appearance Day screen
	            new CShowSetting(0, 0, "Masa Name Format"), // 49 format of masa name
	            new CShowSetting(0, 0, "Ekadasi Parana details"), // 50 ekadasi parana details
	            new CShowSetting(0, 0, "Aniversary show format"), // 51 format of aniversary info
	            new CShowSetting(1, 1, "Sun events"), // 52
	            new CShowSetting(1, 1, "Tithi events"), //53
	            new CShowSetting(1, 1, "Naksatra Events"), //54
	            new CShowSetting(1, 1, "Sankranti Events"),//55
	            new CShowSetting(1, 1, "Conjunction Events"),//56
	            new CShowSetting(0, 0, "Rahu kalam"), //57
	            new CShowSetting(0, 0, "Yama ghanti"), //58
	            new CShowSetting(0, 0, "Guli kalam"), //59
	            new CShowSetting(0, 0, "Moon events"), //60
	            new CShowSetting(0, 0, "Moon rasi"), //61
	            new CShowSetting(0, 0, "Ascendent"), //62
	            new CShowSetting(1, 1, "Sort results core events"),//63
	            new CShowSetting(0, 0, "Abhijit Muhurta"), //64
	            new CShowSetting(0, 0, "Yoga Events"), //65
	            new CShowSetting(1, 1, "Caturmasya System"), //66
                new CShowSetting(0, 0, "Core Events in Calendar"),//67
                new CShowSetting(0, 0, "Calendar Column - Sunrise"),//68
                new CShowSetting(0, 0, "Calendar Column - Noon"),//69
                new CShowSetting(0, 0, "Calendar Column - Sunset"),//70
                new CShowSetting(0, 0, "Festival at first day of tithi"),//71
                new CShowSetting(0, 0, null)
            };
        }

        public static void LoadDisplaySettingsFile(string psz)
        {
            try
            {
                DSCollection = new JavaScriptSerializer().Deserialize<List<GCDisplaySettings>>(psz);
            }
            catch(Exception x)
            {
                Console.WriteLine(x.Message);
            }

                /*if (!File.Exists(psz))
                return;

            XmlDocument doc = new XmlDocument();
            doc.Load(psz);

            DSCollection.Clear();
            foreach(XmlElement e2 in doc.GetElementsByTagName("Settings"))
            {
                GCDisplaySettings ds = new GCDisplaySettings(e2.GetAttribute("name"));
                DSCollection.Add(ds);

                foreach(XmlElement e3 in e2.GetElementsByTagName("Item"))
                {
                    int index;
                    int value;
                    if (int.TryParse(e3.GetAttribute("index"), out index) && int.TryParse(e3.GetAttribute("value"), out value))
                    {
                        ds.setValue(index, value);
                    }
                }
            }*/
        }



        public static void SaveDisplaySettingsFile(string fileName)
        {
            Console.WriteLine("Display Settings saved to: " + fileName);
            var json = new JavaScriptSerializer().Serialize(DSCollection);
            File.WriteAllText(fileName, json);

            /*XmlDocument doc = new XmlDocument();
            XmlElement e1 = doc.CreateElement("DisplaySettings");
            doc.AppendChild(e1);
            foreach(GCDisplaySettings gds in DSCollection)
            {
                XmlElement e2 = doc.CreateElement("Settings");
                e1.AppendChild(e2);

                e2.SetAttribute("name", gds.Name);
                for (int i = 0; i < gds.gss.Length; i++)
                {
                    XmlElement e3 = doc.CreateElement("Item");
                    e2.AppendChild(e3);
                    e3.SetAttribute("index", i.ToString());
                    e3.SetAttribute("value", gds.gss[i].val.ToString());
                    e3.SetAttribute("desc", gds.gss[i].text);
                }
            }
            doc.Save(fileName);*/
        }

        protected static List<GCDisplaySettings> SettingsStack = new List<GCDisplaySettings>();

        protected static GCDisplaySettings p_curr = null;

        private static List<GCDisplaySettings> DSCollection = null;

        static GCDisplaySettings()
        {
            p_curr = new GCDisplaySettings("Default");
            DSCollection = new List<GCDisplaySettings>();
            DSCollection.Add(p_curr);
        }

        public static GCDisplaySettings Current
        {
            get
            {
                return p_curr;
            }
            set
            {
                p_curr = value;
            }
        }

        public void Push()
        {
            SettingsStack.Add(new GCDisplaySettings(Current));
        }

        public void Pop()
        {
            if (SettingsStack.Count > 0)
            {
                Current = SettingsStack[SettingsStack.Count - 1];
                SettingsStack.RemoveAt(SettingsStack.Count - 1);
            }
        }

        public void Clear()
        {
            foreach(CShowSetting ss in gss)
            {
                ss.val = 0;
            }
        }
    }

    public sealed class GCDS
    {
        public static readonly int DISP_ALWAYS = -1;
        public static readonly int CAL_ARUN_TIME = 1;
        public static readonly int CAL_ARUN_TITHI = 0;
        public static readonly int CAL_SUN_RISE = 2;
        public static readonly int CAL_SUN_SANDHYA = 34;
        public static readonly int CAL_BRAHMA_MUHURTA = 3;
        public static readonly int CAL_MOON_RISE = 4;
        public static readonly int CAL_MOON_SET = 5;
        public static readonly int CAL_KSAYA = 7;
        public static readonly int CAL_VRDDHI = 8;
        public static readonly int CAL_SUN_LONG = 9;
        public static readonly int CAL_MOON_LONG = 10;
        public static readonly int CAL_AYANAMSHA = 11;
        public static readonly int CAL_JULIAN = 12;
        public static readonly int CATURMASYA_SYSTEM = 66;
        public static readonly int CATURMASYA_PURNIMA = 13;
        public static readonly int CATURMASYA_PRATIPAT = 14;
        public static readonly int CATURMASYA_EKADASI = 15;
        public static readonly int CAL_SANKRANTI = 16;
        public static readonly int CAL_EKADASI_PARANA = 17;
        public static readonly int CAL_MASA_CHANGE = 21;
        public static readonly int CAL_FEST_0 = 22;
        //public static readonly int CAL_FEST_1 = 23;
        //public static readonly int CAL_FEST_2 = 24;
        //public static readonly int CAL_FEST_3 = 25;
        //public static readonly int CAL_FEST_4 = 26;
        //public static readonly int CAL_FEST_5 = 27;
        //public static readonly int CAL_FEST_6 = 28;
        public static readonly int CAL_DST_CHANGE = 35;
        public static readonly int GENERAL_FIRST_DOW = 40;
        public static readonly int APP_CHILDNAMES = 48;
        public static readonly int GENERAL_MASA_FORMAT = 49;
        public static readonly int GENERAL_ANNIVERSARY_FMT = 51;
        public static readonly int COREEVENTS_SUN = 52;
        public static readonly int COREEVENTS_TITHI = 53;
        public static readonly int COREEVENTS_NAKSATRA = 54;
        public static readonly int COREEVENTS_SANKRANTI = 55;
        public static readonly int COREEVENTS_CONJUNCTION = 56;
        public static readonly int COREEVENTS_RAHUKALAM = 57;
        public static readonly int COREEVENTS_YAMAGHANTI = 58;
        public static readonly int COREEVENTS_GULIKALAM = 59;
        public static readonly int COREEVENTS_MOON = 60;
        public static readonly int COREEVENTS_MOONRASI = 61;
        public static readonly int COREEVENTS_ASCENDENT = 62;
        public static readonly int COREEVENTS_SORT = 63;
        public static readonly int COREEVENTS_ABHIJIT_MUHURTA = 64;
        public static readonly int COREEVENTS_YOGA = 65;
        public static readonly int CAL_COREEVENTS = 67;

    };

    public sealed class DisplayPriorities
    {
        public static readonly int PRIO_MAHADVADASI = 10;
        public static readonly int PRIO_EKADASI = 20;
        public static readonly int PRIO_EKADASI_PARANA = 90;
        public static readonly int PRIO_FESTIVALS_0 = 100;
        public static readonly int PRIO_FESTIVALS_1 = 200;
        public static readonly int PRIO_FESTIVALS_2 = 300;
        public static readonly int PRIO_FESTIVALS_3 = 400;
        public static readonly int PRIO_FESTIVALS_4 = 500;
        public static readonly int PRIO_FESTIVALS_5 = 600;
        public static readonly int PRIO_FESTIVALS_6 = 700;
        public static readonly int PRIO_FASTING = 900;
        public static readonly int PRIO_SANKRANTI = 920;
        public static readonly int PRIO_MASA_CHANGE = 940;
        public static readonly int PRIO_DST_CHANGE = 950;
        public static readonly int PRIO_KSAYA = 965;
        public static readonly int PRIO_CM_CONT = 971;
        public static readonly int PRIO_CM_DAY = 972;
        public static readonly int PRIO_CM_DAYNOTE = 973;
        public static readonly int PRIO_ARUN = 975;
        public static readonly int PRIO_SUN = 980;
        public static readonly int PRIO_MOON = 990;
        public static readonly int PRIO_CORE_ASTRO = 995;
        public static readonly int PRIO_ASTRO = 1000;

    };
}
