using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace GCAL.Base
{
    public class GCStrings
    {
        public static List<String> gstr = new List<string>();
        public static bool gstr_Modified = false;
        public static string FullVersionText { get; set; }
        public static string ShortVersionText { get; set; }
        public static string RawVersionNumber { get; set; }


        private static string[] childsylable = {
		    "chu", "che", "cho", "la", //asvini
		    "li", "lu", "le", "lo", // bharani
		    "a", "i", "u", "e", //krtika
		    "o", "va", "vi", "vo", // rohini
		    "ve", "vo", "ka", "ke", // mrgasira
		    "ku","gha","ng","chha", // ardra
		    "ke","ko","ha","hi", // punarvasu
		    "hu","he","ho","da", // pushya
		    "di","du","de","do", //aslesa
		    "ma","mi","mu","me", //magha
		    "mo","ta","ti","tu", //purvaphalguni
		    "te","to","pa","pi", //uttaraphalguni
		    "pu","sha","na","tha",//hasta
		    "pe","po","ra","ri",//chitra
		    "ru","re","ra","ta",//svati
		    "ti","tu","te","to",//visakha
		    "na","ni","nu","ne",// anuradha
		    "no","ya","yi","yu",//jyestha
		    "ye","yo","ba","bi",// mula
		    "bu","dha","bha","dha",//purvasada
		    "be","bo","ja","ji",// uttarasada
		    "ju","je","jo","gha",//sravana
		    "ga","gi","gu","ge",// dhanistha
		    "go","sa","si","su",//satabhisda
		    "se","so","da","di",//purvabhadra
		    "du","tha","jna","da",//uttarabhadra
		    "de","do","cha","chi"// revati

	    };

        private static string[] childsylableRasi = {
		    "a.. e.. la..",
		    "u.. ba.. va..",
		    "ka.. gha..",
		    "da.. ha..",
		    "ma..",
		    "pa..",
		    "ra.. ta..",
		    "na.. ya..",
		    "dha.. bha... pha..",
		    "kha.. ja..",
		    "ga.. sa.. sha..",
		    "da.. ca.. tha.. jha.."
	    };

        public static string GetNaksatraChildSylable(int n, int pada)
        {
            int i = (n * 4 + pada) % 108;


            return childsylable[i];
        }

        public static string GetRasiChildSylable(int n)
        {
            return childsylableRasi[n % 12];
        }

        public static string GetSpecFestivalName(int i)
        {
            switch (i)
            {
                case SpecialFestivalId.SPEC_JANMASTAMI:
                    return Localized("Sri Krsna Janmastami: Appearance of Lord Sri Krsna");
                case SpecialFestivalId.SPEC_GAURAPURNIMA:
                    return Localized("Gaura Purnima: Appearance of Sri Caitanya Mahaprabhu");
                case SpecialFestivalId.SPEC_RETURNRATHA:
                    return Localized("Return Ratha (8 days after Ratha Yatra)");
                case SpecialFestivalId.SPEC_HERAPANCAMI:
                    return Localized("Hera Pancami (4 days after Ratha Yatra)");
                case SpecialFestivalId.SPEC_GUNDICAMARJANA:
                    return Localized("Gundica Marjana");
                case SpecialFestivalId.SPEC_GOVARDHANPUJA:
                    return Localized("Go Puja. Go Krda. Govardhana Puja");
                case SpecialFestivalId.SPEC_RAMANAVAMI:
                    return Localized("Rama Navami: Appearance of Lord Sri Ramacandra");
                case SpecialFestivalId.SPEC_RATHAYATRA:
                    return Localized("Ratha Yatra");
                case SpecialFestivalId.SPEC_NANDAUTSAVA:
                    return Localized("Nandotsava");
                case SpecialFestivalId.SPEC_PRABHAPP:
                    return Localized("Srila Prabhupada -- Appearance");
                case SpecialFestivalId.SPEC_MISRAFESTIVAL:
                    return Localized("Festival of Jagannatha Misra");
                default:
                    return "";
            }
        }

        public static void SetSpecFestivalName(int i, string szName)
        {
            switch (i)
            {
                case SpecialFestivalId.SPEC_JANMASTAMI:
                    setString(741, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_GAURAPURNIMA:
                    setString(742, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_RETURNRATHA:
                    setString(743, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_HERAPANCAMI:
                    setString(744, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_GUNDICAMARJANA:
                    setString(745, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_GOVARDHANPUJA:
                    setString(746, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_RAMANAVAMI:
                    setString(747, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_RATHAYATRA:
                    setString(748, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_NANDAUTSAVA:
                    setString(749, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_PRABHAPP:
                    setString(759, szName);
                    gstr_Modified = true;
                    break;
                case SpecialFestivalId.SPEC_MISRAFESTIVAL:
                    setString(750, szName);
                    gstr_Modified = true;
                    break;
                default:
                    break;
            }

            return;
        }

        public static string GetFastingName(int fastingID)
        {
            foreach (FastType ft in FastType.Fasts)
            {
                if (ft.FastID == fastingID)
                {
                    return string.Format("({0})", Localized(ft.FastText));
                }
            }

            return "";
        }

        public static string getString(int i)
        {
            if (i < 0 || i >= gstr.Count)
                return string.Empty;

            return gstr[i];
        }

        public static string GetDSTSignature(int nDST)
        {
            return (nDST != 0) ? Localized("DST") : Localized("LT");
        }

        public static void setString(int i, string str)
        {
            gstr[i] = str;
        }

        public static int LoadStringsFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllBytes(filePath, Properties.Resources.strings);
            }

            int v = 0;
            int h;
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            foreach(XmlElement e in doc.GetElementsByTagName("Item"))
            {
                int i = XH.GetXmlInt(e, "idx", -1);
                if (i >= 0)
                {
                    for (h = gstr.Count; h <= i; h++)
                        gstr.Add(string.Empty);
                    gstr[i] = XH.GetXmlString(e, "val", "");
                    v++;
                }
            }

            RawVersionNumber = "11, Build 4";
            ShortVersionText = "GCal " + RawVersionNumber;
            FullVersionText = "Gaurabda Calendar " + RawVersionNumber;

            return v;
        }

        public static void SaveStringsFile(string filePath)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement e = doc.CreateElement("Strings");
            doc.AppendChild(e);

            for (int i = 0; i < gstr.Count; i++)
            {
                if (gstr[i].Length > 0)
                {
                    XmlElement e2 = doc.CreateElement("Item");
                    e.AppendChild(e2);

                    XH.SetXmlInt(e2, "idx", i);
                    XH.SetXmlString(e2, "val", getString(i));
                }
            }

            doc.Save(filePath);
        }

        public static string GetStringFromIntInc(int i)
        {
            return (i + 1).ToString();
        }

        public static string GetDayPartName(int i)
        {
            if (i == 0) return "Day";
            else return "Night";
        }

        public static string GetMoonTimesName(int i)
        {
            if (i == 0) return "Moonrise";
            else return "Moonset";
        }

        public static string GetKalaName(int i)
        {
            switch (i)
            {
                case KalaType.KT_RAHU_KALAM:
                    return Localized("Rahu kalam");
                case KalaType.KT_YAMA_GHANTI:
                    return Localized("Yama ghanti");
                case KalaType.KT_GULI_KALAM:
                    return Localized("Guli kalam");
                case KalaType.KT_ABHIJIT:
                    return Localized("Abhijit muhurta");
                case KalaType.KT_SANDHYA_MIDNIGHT:
                    return Localized("Midnight sandhya");
                case KalaType.KT_SANDHYA_NOON:
                    return Localized("Noon sandhya");
                case KalaType.KT_SANDHYA_SUNRISE:
                    return Localized("Sunrise sandhya");
                case KalaType.KT_SANDHYA_SUNSET:
                    return Localized("Sunset sandhya");
                default:
                    return Localized("Special interval");
            }
        }

        public static string getLongitudeDirectionName(double d)
        {
            if (d < 0.0)
                return Localized("West");

            return Localized("East");
        }

        public static string getLatitudeDirectionName(double d)
        {
            if (d < 0.0)
                return Localized("South");

            return Localized("North");
        }

        public static int getCount()
        {
            return gstr.Count;
        }

        private static string[] p_muhurtaName = new string[] {
            "Rudra", // GCStrings.Localized("Rudra");
            "Ahi", // GCStrings.Localized("Ahi");
            "Mitra", // GCStrings.Localized("Mitra");
            "Pitri", // GCStrings.Localized("Pitri");
            "Vasu", // GCStrings.Localized("Vasu");
            "Varaha", // GCStrings.Localized("Varaha");
            "Visvedeva", // GCStrings.Localized("Visvedeva");
            "Vidhi", // GCStrings.Localized("Vidhi");
            "Sutamukhi", // GCStrings.Localized("Sutamukhi");
            "Puruhuta", // GCStrings.Localized("Puruhuta");
            "Vahini", // GCStrings.Localized("Vahini");
            "Naktanakara", // GCStrings.Localized("Naktanakara");
            "Varuna", // GCStrings.Localized("Varuna");
            "Aryaman", // GCStrings.Localized("Aryaman");
            "Bhaga", // GCStrings.Localized("Bhaga");
            "Girisha", // GCStrings.Localized("Girisha");
            "Ajapada", // GCStrings.Localized("Ajapada");
            "Ahir-Budhnya", // GCStrings.Localized("Ahir-Budhnya");
            "Pusya", // GCStrings.Localized("Pusya");
            "Asvini", // GCStrings.Localized("Asvini");
            "Yama", // GCStrings.Localized("Yama");
            "Agni", // GCStrings.Localized("Agni");
            "Vidhatri", // GCStrings.Localized("Vidhatri");
            "Kanda", // GCStrings.Localized("Kanda");
            "Aditi", // GCStrings.Localized("Aditi");
            "Jiva/Amrta", // GCStrings.Localized("Jiva/Amrta");
            "Visnu", // GCStrings.Localized("Visnu");
            "Dyumadgadyuti", // GCStrings.Localized("Dyumadgadyuti");
            "Brahma", // GCStrings.Localized("Brahma");
            "Samudram" // GCStrings.Localized("Samudram");
        };

        public static string GetMuhurtaName(int i)
        {
            return GCStrings.Localized(p_muhurtaName[i % 30]);
        }

        public static string GetSandhyaName(int i)
        {
            switch (i)
            {
                case 0:
                    return Localized("Sunrise");
                case 1:
                    return Localized("Noon");
                case 2:
                    return Localized("Sunset");
                case 3:
                    return Localized("Midnight");
                default:
                    return "";
            }
        }



        public static string Localized(string s)
        {
            return s;
        }

        private static string[] p_planetsNamesEn =
        {
            "Sun", "Moon", "Mars", "Mercury", "Jupiter", "Venus", "Saturn", "Rahu", "Ketu", "Ascendent"
        };

        private static string[] p_planetsNames =
        {
            "Surya", "Chandra", "Mangala", "Buddha", "Guru", "Sukra", "Sani", "Rahu", "Ketu", "Lagna"
        };

        public static string GetPlanetNameEn(int i)
        {
            return p_planetsNamesEn[i % 12];
        }

        public static string GetPlanetName(int i)
        {
            return p_planetsNames[i % 12];
        }
    }
}
