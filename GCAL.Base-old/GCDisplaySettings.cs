using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

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


        public bool CalColNaksatra { get { return getBoolValue(36); } set { setBoolValue(36, value); } }
        public bool CalColYoga { get { return getBoolValue(37); } set { setBoolValue(37, value); } }
        public bool CalColFast { get { return getBoolValue(38); } set { setBoolValue(38, value); } }
        public bool CalColPaksa { get { return getBoolValue(39); } set { setBoolValue(39, value); } }
        public bool CalColMoonRasi { get { return getBoolValue(41); } set { setBoolValue(41, value); } }
        public bool CalColCoreEvents { get { return getBoolValue(67); } set { setBoolValue(67, value); } }
        public bool CalColSunrise { get { return getBoolValue(68); } set { setBoolValue(68, value); } }
        public bool CalColNoon { get { return getBoolValue(69); } set { setBoolValue(69, value); } }
        public bool CalColSunset { get { return getBoolValue(70); } set { setBoolValue(70, value); } }

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
            return gss[i].val;
        }

        public bool getBoolValue(int i)
        {
            return gss[i].val != 0;
        }
        public void setValue(int i, int val)
        {
            gss[i].val = val;
            gss[i].old_val = val;
        }

        public void setBoolValue(int i, bool val)
        {
            gss[i].val = val ? 1 : 0;
            gss[i].old_val = gss[i].val;
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
	            /* BEGIN GCAL 1.4.3 */
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
	            /* END GCAL 1.4.3 */
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
                new CShowSetting(0, 0, null)
            };
        }

        public static void LoadDisplaySettingsFile(string psz)
        {
            if (!File.Exists(psz))
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
            }
        }



        public static void SaveDisplaySettingsFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
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
            doc.Save(fileName);
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
        public static readonly int CAL_HEADER_MASA = 18;
        public static readonly int CAL_HEADER_MONTH = 19;
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
        public static readonly int CAL_COL_SUNRISE = 68;
        public static readonly int CAL_COL_NOON    = 69;
        public static readonly int CAL_COL_SUNSET  = 70;

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
