using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base
{
    public class GCCalendar
    {
        public enum PeriodUnit
        {
            Days = 1,
            Weeks = 2,
            Months = 3,
            Years = 4,
            Tithis = 5,
            Masas = 6,
            Gaurabda = 7
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dow">from range 0..6</param>
        /// <returns></returns>
        public static string GetWeekdayAbbr(int dow)
        {
            switch (dow)
            {
                case 0: return GCStrings.Localized("Su");
                case 1: return GCStrings.Localized("Mo");
                case 2: return GCStrings.Localized("Tu");
                case 3: return GCStrings.Localized("We");
                case 4: return GCStrings.Localized("Th");
                case 5: return GCStrings.Localized("Fr");
                case 6: return GCStrings.Localized("Sa");
            }
            return "";
        }

        public static string GetWeekdayName(int dow)
        {
            switch (dow)
            {
                case 0: return GCStrings.Localized("Sunday");
                case 1: return GCStrings.Localized("Monday");
                case 2: return GCStrings.Localized("Tuesday");
                case 3: return GCStrings.Localized("Wednesday");
                case 4: return GCStrings.Localized("Thursday");
                case 5: return GCStrings.Localized("Friday");
                case 6: return GCStrings.Localized("Saturday");
            }
            return "";
        }

        public static int GetGaurabdaYear(GregorianDateTime vc, GCEarthData earth)
        {
            GCAstroData day = new GCAstroData();

            day.DayCalc(vc, earth);
            day.MasaCalc(vc, earth);

            return day.GaurabdaYear;
        }

        public static string FormatDate(GregorianDateTime vc, GaurabdaDate va)
        {
            return string.Format("{0} {1} {2}\r\n{3}, {4} Paksa, {5} Masa, {6}",
                vc.day, GregorianDateTime.GetMonthAbreviation(vc.month), vc.year,
                GCTithi.GetName(va.tithi % 15), GCPaksa.GetName(va.tithi / 15), GCMasa.GetName(va.masa), va.gyear);

        }

        //===========================================================================
        //
        //===========================================================================

        public static void VATIMEtoVCTIME(GaurabdaDate va, out GregorianDateTime vc, GCEarthData earth)
        {
            vc = GCTithi.CalcTithiDate(va.gyear, va.masa, va.tithi / 15, va.tithi % 15, earth);
        }

        //===========================================================================
        //
        //===========================================================================

        public static void VCTIMEtoVATIME(GregorianDateTime vc, out GaurabdaDate va, GCEarthData earth)
        {
            GCAstroData day = new GCAstroData();

            day.DayCalc(vc, earth);
            va = new GaurabdaDate();
            va.masa = day.MasaCalc(vc, earth);
            va.tithi = day.sunRise.Tithi;
            va.gyear = day.GaurabdaYear;
        }

        public static int LimitForPeriodUnit(PeriodUnit pu)
        {
            switch (pu)
            {
                case PeriodUnit.Days:
                case PeriodUnit.Tithis:
                    return 36500;
                case PeriodUnit.Weeks:
                    return 520;
                case PeriodUnit.Months:
                case PeriodUnit.Masas:
                    return 1200;
                default:
                    return 100;
            }
        }

        public static bool CalcEndDate(GCEarthData m_earth, GregorianDateTime vcStart, out GregorianDateTime vcEnd, PeriodUnit nType, int nCount)
        {
            if (m_earth == null || vcStart == null)
            {
                vcEnd = null;
                return false;
            }

            GaurabdaDate vaStart = new GaurabdaDate();
            VCTIMEtoVATIME(vcStart, out vaStart, m_earth);
            vcEnd = new GregorianDateTime();
            GaurabdaDate vaEnd = new GaurabdaDate();
            if (nCount > LimitForPeriodUnit(nType))
                nCount = LimitForPeriodUnit(nType);

            switch (nType)
            {
                case PeriodUnit.Days:
                    vcEnd.Set(vcStart);
                    vcEnd.AddDays(nCount);
                    VCTIMEtoVATIME(vcEnd, out vaEnd, m_earth);
                    break;
                case PeriodUnit.Weeks:
                    vcEnd.Set(vcStart);
                    vcEnd.AddDays(nCount * 7);
                    VCTIMEtoVATIME(vcEnd, out vaEnd, m_earth);
                    break;
                case PeriodUnit.Months:
                    vcEnd.Set(vcStart);
                    vcEnd.month += nCount;
                    while (vcEnd.month > 12)
                    {
                        vcEnd.year++;
                        vcEnd.month -= 12;
                    }
                    VCTIMEtoVATIME(vcEnd, out vaEnd, m_earth);
                    break;
                case PeriodUnit.Years:
                    vcEnd.Set(vcStart);
                    vcEnd.year += nCount;
                    VCTIMEtoVATIME(vcEnd, out vaEnd, m_earth);
                    break;
                case PeriodUnit.Tithis:
                    vaEnd.Set(vaStart);
                    vaEnd.tithi += nCount;
                    while (vaEnd.tithi >= 30)
                    {
                        vaEnd.tithi -= 30;
                        vaEnd.masa++;
                    }
                    while (vaEnd.masa >= 12)
                    {
                        vaEnd.masa -= 12;
                        vaEnd.gyear++;
                    }
                    VATIMEtoVCTIME(vaEnd, out vcEnd, m_earth);
                    break;
                case PeriodUnit.Masas:
                    vaEnd.Set(vaStart);
                    vaEnd.masa = MasaToComboMasa(vaEnd.masa);
                    if (vaEnd.masa == (int)MasaId.ADHIKA_MASA)
                    {
                        vcEnd.Set(vcStart);
                        vcEnd.month += nCount;
                        while (vcEnd.month > 12)
                        {
                            vcEnd.year++;
                            vcEnd.month -= 12;
                        }
                        VCTIMEtoVATIME(vcEnd, out vaEnd, m_earth);
                        vaEnd.tithi = vaStart.tithi;
                        VATIMEtoVCTIME(vaEnd, out vcEnd, m_earth);
                    }
                    else
                    {
                        vaEnd.masa += nCount;
                        while (vaEnd.masa >= 12)
                        {
                            vaEnd.masa -= 12;
                            vaEnd.gyear++;
                        }
                        vaEnd.masa = ComboMasaToMasa(vaEnd.masa);
                        VATIMEtoVCTIME(vaEnd, out vcEnd, m_earth);
                    }
                    break;
                case PeriodUnit.Gaurabda:
                    vaEnd.Set(vaStart);
                    vaEnd.gyear += nCount;
                    VATIMEtoVCTIME(vaEnd, out vcEnd, m_earth);
                    break;
            }

            return true;
        }

        public static int ComboMasaToMasa(int nComboMasa)
        {
            return (nComboMasa == 12) ? 12 : ((nComboMasa + 11) % 12);
        }

        public static int MasaToComboMasa(int nMasa)
        {
            return (nMasa == 12) ? 12 : ((nMasa + 1) % 12);
        }

        public static int writeFirstDayXml(string fileName, GCLocation loc, GregorianDateTime vcStart)
        {
            using (StreamWriter xml = new StreamWriter(fileName))
            {

                vcStart.Set(GCAstroData.GetFirstDayOfYear(loc.GetEarthData(), vcStart.year));
                vcStart.InitWeekDay();

                // write
                xml.Write("<xml>\n");
                xml.Write("\t<request name=\"FirstDay\" version=\"");
                xml.Write(GCStrings.RawVersionNumber);
                xml.Write("\">\n");
                xml.Write("\t\t<arg name=\"longitude\" val=\"");
                xml.Write(loc.Longitude);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"latitude\" val=\"");
                xml.Write(loc.Latitude);
                xml.Write("\" />\n");
                xml.Write("\t\t<arg name=\"year\" val=\"");
                xml.Write(vcStart.year);
                xml.Write("\" />\n");
                xml.Write("\t</request>\n");
                xml.Write("\t<result name=\"FirstDay_of_GaurabdaYear\">\n");
                xml.Write("\t\t<firstday date=\"");
                xml.Write(vcStart);
                xml.Write("\" dayweekid = \"");
                xml.Write(vcStart.dayOfWeek);
                xml.Write("\" dayweek=\"");
                xml.Write(GCCalendar.GetWeekdayName(vcStart.dayOfWeek));
                xml.Write("\" />\n");
                xml.Write("\t</result>\n");
                xml.Write("</xml>\n");
            }
            return 0;
        }


    }
}
