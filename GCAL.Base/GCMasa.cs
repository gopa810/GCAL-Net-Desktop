using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCMasa
    {
        private static string[] p_masaNameGaudiya = new string[] {
            "Madhusudana", // GCStrings.Localized("Madhusudana");
            "Trivikrama", // GCStrings.Localized("Trivikrama");
            "Vamana", // GCStrings.Localized("Vamana");
            "Sridhara", // GCStrings.Localized("Sridhara");
            "Hrsikesa", // GCStrings.Localized("Hrsikesa");
            "Padmanabha", // GCStrings.Localized("Padmanabha");
            "Damodara", // GCStrings.Localized("Damodara");
            "Kesava", // GCStrings.Localized("Kesava");
            "Narayana", // GCStrings.Localized("Narayana");
            "Madhava", // GCStrings.Localized("Madhava");
            "Govinda", // GCStrings.Localized("Govinda");
            "Visnu", // GCStrings.Localized("Visnu");
            "Purusottama-adhika", // GCStrings.Localized("Purusottama-adhika");
        };

        private static string[] p_masaNameVedic = new string[] {
            "Vaisakha", // GCStrings.Localized("Vaisakha");
            "Jyestha", // GCStrings.Localized("Jyestha");
            "Asadha", // GCStrings.Localized("Asadha");
            "Sravana", // GCStrings.Localized("Sravana");
            "Bhadra", // GCStrings.Localized("Bhadra");
            "Asvina", // GCStrings.Localized("Asvina");
            "Kartika", // GCStrings.Localized("Kartika");
            "Margasirsa", // GCStrings.Localized("Margasirsa");
            "Pausa", // GCStrings.Localized("Pausa");
            "Magha", // GCStrings.Localized("Magha");
            "Phalguna", // GCStrings.Localized("Phalguna");
            "Caitra", // GCStrings.Localized("Caitra");
            "Adhika", // GCStrings.Localized("Adhika");
        };

        /// <summary>
        /// Returns Gaudiya style of name of the month
        /// </summary>
        /// <param name="masaID">0..Madhusudana, 12..Purusottama-adhika</param>
        /// <returns></returns>
        private static string GetGaudiyaName(int masaID)
        {
            return GCStrings.Localized(p_masaNameGaudiya[masaID % 13]);
        }

        /// <summary>
        /// Returns Vedic style of name of the month
        /// </summary>
        /// <param name="masaID"></param>
        /// <returns></returns>
        private static string GetVedicName(int masaID)
        {
            return GCStrings.Localized(p_masaNameVedic[masaID % 13]);
        }

        /// <summary>
        /// Returns combined name of month according user settings
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetName(int i)
        {
            return GetNameEx(i, GCDisplaySettings.Current.getValue(49));
        }

        public static string GetNameEx(int masaIndex, int formatIndex)
        {
            switch (formatIndex)
            {
                case 0: return GetGaudiyaName(masaIndex);
                case 1: return string.Format("{0} ({1})", GetGaudiyaName(masaIndex), GetVedicName(masaIndex));
                case 2: return GetVedicName(masaIndex);
                case 3: return string.Format("{0} ({1})", GetVedicName(masaIndex), GetGaudiyaName(masaIndex));
                default: return GetGaudiyaName(masaIndex);
            }
        }

        public static int PREV_MASA(int nMasa)
        {
            return (nMasa + 11) % 12;
        }

        public static int NEXT_MASA(int nMasa)
        {
            return (nMasa + 1) % 12;
        }

        public static bool IS_EXTRA(int nMasa)
        {
            return nMasa == 12;
        }

        public static int MASA_DIST(int nMasa, int nMasa2)
        {
            int a = (nMasa - nMasa2);
            while (a < -6)
                a += 12;
            return a;
        }
    }
}
