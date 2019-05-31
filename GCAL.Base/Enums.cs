using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public sealed class MahadvadasiType
    {
        public const int EV_NULL = 0x100;
        public const int EV_SUDDHA = 0x101;
        public const int EV_UNMILANI = 0x102;
        public const int EV_VYANJULI = 0x103;
        public const int EV_TRISPRSA = 0x104;
        public const int EV_UNMILANI_TRISPRSA = 0x105;
        public const int EV_PAKSAVARDHINI = 0x106;
        public const int EV_JAYA = 0x107;
        public const int EV_JAYANTI = 0x108;
        public const int EV_PAPA_NASINI = 0x109;
        public const int EV_VIJAYA = 0x110;
    };


    public sealed class FastType
    {
        public const int FAST_NULL = 0;
        public const int FAST_NOON = 1;
        public const int FAST_SUNSET = 2;
        public const int FAST_MOONRISE = 3;
        public const int FAST_DUSK = 4;
        public const int FAST_MIDNIGHT = 5;
        public const int FAST_EKADASI = 6;
        public const int FAST_DAY = 7;
        public const int FAST_END_TITHI = 8;
        public const int FAST_END_NAKSATRA = 9;
        public const int FAST_END_TAN = 10;
        public const int FAST_END_TON = 11;

        public int FastID;
        public string FastText;

        /// <summary>
        /// Strings for user interface sorted by frequency of use
        /// </summary>
        public static FastType[] Fasts = new FastType[] {
            new FastType(){ FastID = FAST_NULL, FastText = "No fasting"},
            new FastType(){ FastID = FAST_DAY, FastText = "Fast today"},
            new FastType(){ FastID = FAST_EKADASI, FastText = "Ekadasi fasting"},
            new FastType(){ FastID = FAST_NOON, FastText = "Fast till noon"},
            new FastType(){ FastID = FAST_SUNSET, FastText = "Fast till sunset"},
            new FastType(){ FastID = FAST_MOONRISE, FastText = "Fast till moonrise"},
            new FastType(){ FastID = FAST_DUSK, FastText = "Fast till dusk"},
            new FastType(){ FastID = FAST_MIDNIGHT, FastText = "Fast till midnight"},
            new FastType(){ FastID = FAST_END_TITHI, FastText = "Fast till end of tithi"},
            new FastType(){ FastID = FAST_END_NAKSATRA, FastText = "Fast till end of naksatra"},
            new FastType(){ FastID = FAST_END_TAN, FastText = "Fast till end of tithi and naksatra"},
            new FastType(){ FastID = FAST_END_TON, FastText = "Fast till end of tithi or naksatra"}
        };

        public override string ToString()
        {
            return FastText;
        }
    };

    public sealed class FeastType
    {
        public const int FEAST_NULL = 0;
        public const int FEAST_TODAY_FAST_YESTERDAY = 1;
        public const int FEAST_TOMMOROW_FAST_TODAY = 2;
    };

    public sealed class SpecialFestivalId
    {
        public const int SPEC_RETURNRATHA = 3;
        public const int SPEC_HERAPANCAMI = 4;
        public const int SPEC_GUNDICAMARJANA = 5;
        public const int SPEC_GOVARDHANPUJA = 6;
        public const int SPEC_VAMANADVADASI = 7;
        public const int SPEC_VARAHADVADASI = 8;
        public const int SPEC_RAMANAVAMI = 9;
        public const int SPEC_JANMASTAMI = 10;
        public const int SPEC_RATHAYATRA = 11;
        public const int SPEC_GAURAPURNIMA = 12;
        public const int SPEC_NANDAUTSAVA = 13;
        public const int SPEC_MISRAFESTIVAL = 14;
        public const int SPEC_PRABHAPP = 15;
    };


    public sealed class EkadasiParanaType
    {
        public const int EP_TYPE_NULL = 0;
        public const int EP_TYPE_3DAY = 1;
        public const int EP_TYPE_4TITHI = 2;
        public const int EP_TYPE_NAKEND = 3;
        public const int EP_TYPE_SUNRISE = 4;
        public const int EP_TYPE_TEND = 5;
    };

    public sealed class CaturmasyaCodes
    {
        public const int CMASYA_SYSTEM_PURNIMA = 13;
        public const int CMASYA_SYSTEM_PRATIPAT = 14;
        public const int CMASYA_SYSTEM_EKADASI = 15;
        public const int CMASYA_SYSTEM_MASK = 0xff;

        public const int CMASYA_MONTH_1 = 0x100;
        public const int CMASYA_MONTH_2 = 0x200;
        public const int CMASYA_MONTH_3 = 0x300;
        public const int CMASYA_MONTH_4 = 0x400;
        public const int CMASYA_MONTH_MASK = 0xf00;

        public const int CMASYA_DAY_FIRST = 0x1000;
        public const int CMASYA_DAY_LAST = 0x2000;
        public const int CMASYA_DAY_MASK = 0xf000;
    };


    public sealed class MasaId
    {
        public const int MADHUSUDANA_MASA = 0;
        public const int TRIVIKRAMA_MASA = 1;
        public const int VAMANA_MASA = 2;
        public const int SRIDHARA_MASA = 3;
        public const int HRSIKESA_MASA = 4;
        public const int PADMANABHA_MASA = 5;
        public const int DAMODARA_MASA = 6;
        public const int KESAVA_MASA = 7;
        public const int NARAYANA_MASA = 8;
        public const int MADHAVA_MASA = 9;
        public const int GOVINDA_MASA = 10;
        public const int VISNU_MASA = 11;
        public const int ADHIKA_MASA = 12;
    };

    public sealed class SankrantiId
    {
        public const int MESHA_SANKRANTI = 0;
        public const int VRSABHA_SANKRANTI = 1;
        public const int MITHUNA_SANKRANTI = 2;
        public const int KATAKA_SANKRANTI = 3;
        public const int SIMHA_SANKRANTI = 4;
        public const int KANYA_SANKRANTI = 5;
        public const int TULA_SANKRANTI = 6;
        public const int VRSCIKA_SANKRANTI = 7;
        public const int DHANUS_SANKRANTI = 8;
        public const int MAKARA_SANKRANTI = 9;
        public const int KUMBHA_SANKRANTI = 10;
        public const int MINA_SANKRANTI = 11;
    };

    public sealed class PaksaId
    {
        public const int KRSNA_PAKSA = 0;
        public const int GAURA_PAKSA = 1;
    };

    public sealed class TithiId
    {
        public const int TITHI_KRSNA_PRATIPAT = 0;
        public const int TITHI_KRSNA_DVITIYA = 1;
        public const int TITHI_KRSNA_TRITIYA = 2;
        public const int TITHI_KRSNA_CATURTI = 3;
        public const int TITHI_KRSNA_PANCAMI = 4;
        public const int TITHI_KRSNA_SASTI = 5;
        public const int TITHI_KRSNA_SAPTAMI = 6;
        public const int TITHI_KRSNA_ASTAMI = 7;
        public const int TITHI_KRSNA_NAVAMI = 8;
        public const int TITHI_KRSNA_DASAMI = 9;
        public const int TITHI_KRSNA_EKADASI = 10;
        public const int TITHI_KRSNA_DVADASI = 11;
        public const int TITHI_KRSNA_TRAYODASI = 12;
        public const int TITHI_KRSNA_CATURDASI = 13;
        public const int TITHI_AMAVASYA = 14;
        public const int TITHI_GAURA_PRATIPAT = 15;
        public const int TITHI_GAURA_DVITIYA = 16;
        public const int TITHI_GAURA_TRITIYA = 17;
        public const int TITHI_GAURA_CATURTI = 18;
        public const int TITHI_GAURA_PANCAMI = 19;
        public const int TITHI_GAURA_SASTI = 20;
        public const int TITHI_GAURA_SAPTAMI = 21;
        public const int TITHI_GAURA_ASTAMI = 22;
        public const int TITHI_GAURA_NAVAMI = 23;
        public const int TITHI_GAURA_DASAMI = 24;
        public const int TITHI_GAURA_EKADASI = 25;
        public const int TITHI_GAURA_DVADASI = 26;
        public const int TITHI_GAURA_TRAYODASI = 27;
        public const int TITHI_GAURA_CATURDASI = 28;
        public const int TITHI_PURNIMA = 29;
    };

    public sealed class NaksatraId
    {
        public const int ROHINI_NAKSATRA = 3;
    };

    public sealed class  WeekDayId
    {
        public const int DW_SUNDAY = 0;
        public const int DW_MONDAY = 1;
        public const int DW_TUESDAY = 2;
        public const int DW_WEDNESDAY = 3;
        public const int DW_THURSDAY = 4;
        public const int DW_FRIDAY = 5;
        public const int DW_SATURDAY = 6;
    };


    public sealed class KalaType
    {
        public const int KT_NONE = 0;
        public const int KT_RAHU_KALAM = 1;
        public const int KT_YAMA_GHANTI = 2;
        public const int KT_GULI_KALAM = 3;
        public const int KT_VISHAGATI = 4;
        public const int KT_ABHIJIT = 5;
        public const int KT_SANDHYA_SUNRISE = 6;
        public const int KT_SANDHYA_NOON = 7;
        public const int KT_SANDHYA_SUNSET = 8;
        public const int KT_SANDHYA_MIDNIGHT = 9;
    };


}
