using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCEkadasi
    {
        public static string GetMahadvadasiName(int i)
        {
            switch (i)
            {
                case MahadvadasiType.EV_NULL:
                case MahadvadasiType.EV_SUDDHA:
                    return null;
                case MahadvadasiType.EV_UNMILANI:
                    return GCStrings.Localized("Unmilani Mahadvadasi");
                case MahadvadasiType.EV_TRISPRSA:
                case MahadvadasiType.EV_UNMILANI_TRISPRSA:
                    return GCStrings.Localized("Trisprsa Mahadvadasi");
                case MahadvadasiType.EV_PAKSAVARDHINI:
                    return GCStrings.Localized("Paksa vardhini Mahadvadasi");
                case MahadvadasiType.EV_JAYA:
                    return GCStrings.Localized("Jaya Mahadvadasi");
                case MahadvadasiType.EV_VIJAYA:
                    return GCStrings.Localized("Vijaya Mahadvadasi");
                case MahadvadasiType.EV_PAPA_NASINI:
                    return GCStrings.Localized("Papa Nasini Mahadvadasi");
                case MahadvadasiType.EV_JAYANTI:
                    return GCStrings.Localized("Jayanti Mahadvadasi");
                case MahadvadasiType.EV_VYANJULI:
                    return GCStrings.Localized("Vyanjuli Mahadvadasi");
                default:
                    return "(Unknown Mahadvadasi)";
            }
        }

        public static string[] p_ekadasiName = new string[] {
            "Varuthini Ekadasi", // GCStrings.Localized("Varuthini Ekadasi");
            "Mohini Ekadasi", // GCStrings.Localized("Mohini Ekadasi");
            "Apara Ekadasi", // GCStrings.Localized("Apara Ekadasi");
            "Pandava Nirjala Ekadasi", // GCStrings.Localized("Pandava Nirjala Ekadasi");
            "Yogini Ekadasi", // GCStrings.Localized("Yogini Ekadasi");
            "Sayana Ekadasi", // GCStrings.Localized("Sayana Ekadasi");
            "Kamika Ekadasi", // GCStrings.Localized("Kamika Ekadasi");
            "Pavitraropana Ekadasi", // GCStrings.Localized("Pavitraropana Ekadasi");
            "Annada Ekadasi", // GCStrings.Localized("Annada Ekadasi");
            "Parsva Ekadasi", // GCStrings.Localized("Parsva Ekadasi");
            "Indira Ekadasi", // GCStrings.Localized("Indira Ekadasi");
            "Pasankusa Ekadasi", // GCStrings.Localized("Pasankusa Ekadasi");
            "Rama Ekadasi", // GCStrings.Localized("Rama Ekadasi");
            "Utthana Ekadasi", // GCStrings.Localized("Utthana Ekadasi");
            "Utpanna Ekadasi", // GCStrings.Localized("Utpanna Ekadasi");
            "Moksada Ekadasi", // GCStrings.Localized("Moksada Ekadasi");
            "Saphala Ekadasi", // GCStrings.Localized("Saphala Ekadasi");
            "Putrada Ekadasi", // GCStrings.Localized("Putrada Ekadasi");Ramayana23#
            "Sat-tila Ekadasi", // GCStrings.Localized("Sat-tila Ekadasi");
            "Bhaimi Ekadasi", // GCStrings.Localized("Bhaimi Ekadasi");
            "Vijaya Ekadasi", // GCStrings.Localized("Vijaya Ekadasi");
            "Amalaki vrata Ekadasi", // GCStrings.Localized("Amalaki vrata Ekadasi");
            "Papamocani Ekadasi", // GCStrings.Localized("Papamocani Ekadasi");
            "Kamada Ekadasi", // GCStrings.Localized("Kamada Ekadasi");
            "Parama Ekadasi", // GCStrings.Localized("Parama Ekadasi");
            "Padmini Ekadasi" // GCStrings.Localized("Padmini Ekadasi");
        };

        public static string GetEkadasiName(int nMasa, int nPaksa)
        {
            return GCStrings.Localized(p_ekadasiName[(nMasa * 2 + nPaksa) % 26]);
        }

        public static string GetParanaReasonText(int eparana_type)
        {
            switch (eparana_type)
            {
                case CoreEventType.CCTYPE_THIRD_OF_DAY:
                    return GCStrings.Localized("1/3 of daylight");
                case CoreEventType.CCTYPE_TITHI_QUARTER:
                    return GCStrings.Localized("1/4 of tithi");
                case CoreEventType.CCTYPE_S_RISE:
                    return GCStrings.Localized("sunrise");
                case CoreEventType.CCTYPE_TITHI_END:
                    return GCStrings.Localized("end of tithi");
                case CoreEventType.CCTYPE_NAKS_END:
                    return GCStrings.Localized("end of naksatra");
                default:
                    break;
            }

            return "";
        }
    }
}
