using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCUserInterface: GSCore
    {
        private static GCUserInterface p_shared = null;

        public static GCUserInterface Shared
        {
            get
            {
                if (p_shared == null) p_shared = new GCUserInterface();
                return p_shared;
            }
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            GSCore result = GSCore.Void;

            switch (token)
            {
                case "canDisplay":
                    int disp = (int)args.getSafe(0).getIntegerValue();
                    bool b = (disp != 0 && (disp == -1 || GCDisplaySettings.Current.getValue(disp) != 0));
                    result = new GSBoolean(b);
                    break;
                case "getDispValue":
                    result = new GSNumber(GCDisplaySettings.Current.getValue((int)args.getSafe(0).getIntegerValue()));
                    break;
                case "new":
                    switch(args.getSafe(0).getStringValue())
                    {
                        case "GregorianDateTime":
                            result = new GregorianDateTime();
                            break;
                        case "DateTime":
                            result = new GSDateTime();
                            break;
                        default:
                            result = GSCore.Void;
                            break;
                    }
                    break;
                case "calendarHeaderType":
                    result = new GSNumber(GCDisplaySettings.Current.getValue(GCDS.CAL_HEADER_MASA));
                    break;
                case "centerText":
                    string text = args.getSafe(0).getStringValue();
                    int length = (int)args.getSafe(1).getIntegerValue();
                    string padStr = args.getSafe(2).getStringValue();
                    char padChar = padStr.Length > 0 ? padStr[0] : ' ';
                    int half = (length - text.Length) / 2;
                    if (half > 0)
                        result = new GSString(string.Format("{0} {1} {0}", "".PadLeft(half-1, padChar), text));
                    else
                        result = new GSString(text);
                    break;
                case "getTabPos":
                    double d = args.getSafe(0).getDoubleValue();
                    result = new GSNumber(d * GCLayoutData.textSizeText / 24.0);
                    break;
                case "getWeekdayName":
                    result = new GSString(GCCalendar.GetWeekdayName((int)args.getSafe(0).getIntegerValue()));
                    break;
                case "getTimezoneOffset":
                    {
                        string tzname = args.getSafe(0).getStringValue();
                        TTimeZone tzid = TTimeZone.FindTimeZoneByName(tzname);
                        int tzoffset = tzid.OffsetMinutes;
                        char sign = (tzoffset > 0) ? '+' : '-';
                        tzoffset = Math.Abs(tzoffset);
                        result = new GSString(string.Format("{0}{1:00}{2:00}", sign, tzoffset / 60, Math.Abs(tzoffset) % 60));
                    }
                    break;
                case "getTimezoneOffsetBias":
                    {
                        string tzname = args.getSafe(0).getStringValue();
                        TTimeZone tzid = TTimeZone.FindTimeZoneByName(tzname);
                        int tzoffset = tzid.OffsetMinutes + tzid.BiasMinutes;
                        char sign = (tzoffset > 0) ? '+' : '-';
                        tzoffset = Math.Abs(tzoffset);
                        result = new GSString(string.Format("{0}{1:00}{2:00}", sign, tzoffset / 60, Math.Abs(tzoffset) % 60));
                    }
                    break;
                case "getDaylightTimeStartDate":
                    {
                        GregorianDateTime vc = new GregorianDateTime();
                        string loc_timezoneId = args.getSafe(0).getStringValue();
                        int year = (int)args.getSafe(1).getIntegerValue();
                        TTimeZone tzid = TTimeZone.FindTimeZoneByName(loc_timezoneId);
                        tzid.GetDaylightTimeStartDate(year, ref vc);
                        result = vc;
                    }
                    break;
                case "getNormalTimeStartDate":
                    {
                        GregorianDateTime vc = new GregorianDateTime();
                        string loc_timezoneId = args.getSafe(0).getStringValue();
                        int year = (int)args.getSafe(1).getIntegerValue();
                        TTimeZone tzid = TTimeZone.FindTimeZoneByName(loc_timezoneId);
                        tzid.GetNormalTimeStartDate(year, ref vc);
                        result = vc;
                    }
                    break;
                default:
                    result = base.ExecuteMessage(token, args);
                    break;
            }

            return result;
        }

        public override GSCore GetPropertyValue(string Token)
        {
            switch (Token)
            {
                case "versionName":
                    return new GSString() { Value = GCStrings.FullVersionText };
                case "versionShort":
                    return new GSString() { Value = GCStrings.ShortVersionText };
                case "version":
                    return new GSString() { Value = GCStrings.RawVersionNumber };
                case "fontSizeH1":
                    return new GSNumber(GCLayoutData.textSizeH1);
                case "fontSizeH2": 
                    return new GSNumber(GCLayoutData.textSizeH2);
                case "fontSizeText": 
                    return new GSNumber(GCLayoutData.textSizeText);
                case "fontSizeNote":
                    return new GSNumber(GCLayoutData.textSizeNote);

                default:
                    break;
            }
            return base.GetPropertyValue(Token);
        }

        public static int CalculateCalendar(TResultCalendar daybuff, GCLocation loc, GregorianDateTime date, int nDaysCount)
        {
            bool bCalcMoon = (GCDisplaySettings.Current.getValue(4) > 0 || GCDisplaySettings.Current.getValue(5) > 0);

            //GCUserInterface.CreateProgressWindow();

            if (daybuff.CalculateCalendar(loc, date, nDaysCount) == 0)
                return 0;

            //GCUserInterface.CloseProgressWindow();

            return 1;
        }

        public static void CreateProgressWindow()
        {
            if (dcp != null)
            {
                dcp.ShowWindow();
            }
        }

        public static int SetProgressWindowRange(int nMin, int nMax)
        {
            if (dcp != null)
            {
                return dcp.SetRange(nMin, nMax);
            }
            return 0;
        }

        public static void SetProgressWindowPos(double nPos)
        {
            if (dcp != null)
                dcp.SetPos(nPos);
        }

        public static void CloseProgressWindow()
        {
            if (dcp != null)
                dcp.CloseWindow();
        }

        public static void ShowHelp(string pszFile)
        {
            if (windowController != null)
                windowController.ExecuteMessage("showHelp",  new GSString(pszFile));
        }

        public static GSCore windowController;
        public static ICalculationProgressWindow dcp;
        public static int dstSelectionMethod = 2;
        private static int _ShowMode = 1;
        public static int ShowMode
        {
            get {
                return _ShowMode; 
            }
            set { 
                _ShowMode = value; 
            }
        }
    }

    public interface ICalculationProgressWindow
    {
        void ShowWindow();
        int SetRange(int a, int b);
        int SetPos(double c);
        int CloseWindow();
    }

}
