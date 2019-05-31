using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;
using GCAL.CompositeViews;

namespace GCAL
{
    public sealed class MainFrameContentType
    {
        public const int MW_MODE_CAL = 1;
        public const int MW_MODE_EVENTS = 2;
        public const int MW_MODE_MASALIST = 3;
        public const int MW_MODE_RATEDEVENTS = 4;
        public const int MW_MODE_APPDAY = 6;
        public const int MW_MODE_TODAY = 7;
    };

    public class FrameMainController: GVCore
    {
        FrameMain frame;
        ApplicationTabController appTab = null;
        SearchTabController searchTab = null;
        CalendarTabController calendarTab = null;
        CoreEventsTabController coreEventsTab = null;
        AppDayTabController appDayTab = null;
        MasaListTabController masaTab = null;
        TodayTabController todayTab = null;
        RatedEventsTabController ratedTab = null;
        TResultBase currentObject = null;

        public bool DisplaySettingsChanged = false;

        public FrameMainController(FrameMain inframe)
        {
            frame = inframe;
        }

        public TResultBase getCurrentObject()
        {
            return currentObject;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            switch (token)
            {
                case "showTipAtStartup":
                    if (Properties.Settings.Default.ShowStartupTips)
                    {
                        TipOfDayPanelController dc = new TipOfDayPanelController(new HelpTipOfDayPanel());
                        dc.ViewContainer = ViewContainer;
                        ViewContainer.AddControl(dc, GVControlAlign.Center);
                    }
                    break;
                case "showTipOfTheDay":
                    {
                        TipOfDayPanelController dc = new TipOfDayPanelController(new HelpTipOfDayPanel());
                        dc.ViewContainer = ViewContainer;
                        ViewContainer.AddControl(dc, GVControlAlign.Center);
                    }
                    break;
                case "getMainRectangle":
                    Rectangle rc = frame.Bounds;
                    string rs = string.Format("{0}|{1}|{2}|{3}", rc.Left, rc.Top, rc.Right, rc.Bottom);
                    return new GSString(rs);
                case "setMainRectangle":
                    {
                        GSCore a2 = args.getSafe(0);
                        string s = a2.getStringValue();
                        string[] sa = s.Split('|');
                        if (sa.Length == 4)
                        {
                            int[] si = new int[4];
                            int res = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                if (int.TryParse(sa[j], out si[j]))
                                    res++;
                            }
                            if (res == 4)
                            {
                                Rectangle r = new Rectangle(si[0], si[1], si[2], si[3]);
                                frame.Bounds = r;
                            }
                        }
                    }
                    break;
                case "printContents":
                    {
                        TResultBase tb = args.getSafe(0) as TResultBase;
                        if (tb != null)
                        {
                            frame.PrintContentPlain(tb);
                        }
                    }
                    break;
                case GVTabBanner.MsgTabWillHide:
                    {
                        GSCore a2 = args.getSafe(0);
                        switch (a2.getStringValue())
                        {
                            case "app":
                                if (appTab != null)
                                {
                                    appTab.ExecuteMessage("saveSettings");
                                    DisplaySettingsChanged = true;
                                    frame.RefreshTitleText();
                                }
                                break;
                        }
                    }
                    break;
                case GVTabBanner.MsgTabChanged:
                    {
                        GSCore a2 = args.getSafe(0);
                        switch (a2.getStringValue())
                        {
                            case "app":
                                if (appTab == null)
                                {
                                    ApplicationTab at = new ApplicationTab();
                                    appTab = new ApplicationTabController(at);
                                }
                                frame.TabBanner.RemoveAll();
                                appTab.ShowInContainer(frame.TabBanner, GVControlAlign.Fill);
                                currentObject = appTab.ExecuteMessage("getCurrentContent") as TResultBase;
                                break;
                            case "search":
                                if (searchTab == null)
                                    searchTab = new SearchTabController(new SearchTab());
                                frame.TabBanner.RemoveAll();
                                searchTab.ShowInContainer(frame.TabBanner, GVControlAlign.Fill);
                                break;
                            case "cal":
                                if (calendarTab == null)
                                    calendarTab = new CalendarTabController(new CalendarTab());
                                frame.TabBanner.RemoveAll();
                                calendarTab.ShowInContainer(frame.TabBanner, GVControlAlign.Fill);
                                currentObject = calendarTab.ExecuteMessage("getCurrentContent") as TResultBase;
                                if (DisplaySettingsChanged)
                                {
                                    calendarTab.getView().Recalculate();
                                    DisplaySettingsChanged = false;
                                }
                                break;
                            case "events":
                                if (coreEventsTab == null)
                                    coreEventsTab = new CoreEventsTabController(new CoreEventsTab());
                                frame.TabBanner.RemoveAll();
                                coreEventsTab.ShowInContainer(frame.TabBanner, GVControlAlign.Fill);
                                currentObject = coreEventsTab.ExecuteMessage("getCurrentContent") as TResultBase;
                                if (DisplaySettingsChanged)
                                {
                                    coreEventsTab.getView().Recalculate();
                                    DisplaySettingsChanged = false;
                                }
                                break;
                            case "appday":
                                if (appDayTab == null)
                                {
                                    AppDayTab adt = new AppDayTab();
                                    appDayTab = new AppDayTabController(adt);
                                }
                                frame.TabBanner.RemoveAll();
                                appDayTab.ShowInContainer(frame.TabBanner, GVControlAlign.Fill);
                                currentObject = appDayTab.ExecuteMessage("getCurrentContent") as TResultBase;
                                if (DisplaySettingsChanged)
                                {
                                    appDayTab.getView().Recalculate();
                                    DisplaySettingsChanged = false;
                                }
                                break;
                            case "masalist":
                                if (masaTab == null)
                                    masaTab = new MasaListTabController(new MasaListTab());
                                frame.TabBanner.RemoveAll();
                                masaTab.ShowInContainer(frame.TabBanner, GVControlAlign.Fill);
                                currentObject = masaTab.ExecuteMessage("getCurrentContent") as TResultBase;
                                if (DisplaySettingsChanged)
                                {
                                    masaTab.getView().Recalculate();
                                    DisplaySettingsChanged = false;
                                }
                                break;
                            case "today":
                                if (todayTab == null)
                                    todayTab = new TodayTabController(new TodayTab());
                                frame.TabBanner.RemoveAll();
                                todayTab.ShowInContainer(frame.TabBanner, GVControlAlign.Fill);
                                currentObject = todayTab.ExecuteMessage("getCurrentContent") as TResultBase;
                                if (DisplaySettingsChanged)
                                {
                                    todayTab.getView().Recalculate();
                                    DisplaySettingsChanged = false;
                                }
                                break;
                            case "ratedevents":
                                if (ratedTab == null)
                                    ratedTab = new RatedEventsTabController(new RatedEventsTab());
                                frame.TabBanner.RemoveAll();
                                ratedTab.ShowInContainer(frame.TabBanner, GVControlAlign.Fill);
                                currentObject = ratedTab.ExecuteMessage("getCurrentContent") as TResultBase;
                                if (DisplaySettingsChanged)
                                {
                                    ratedTab.getView().Recalculate();
                                    DisplaySettingsChanged = false;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "print":
                    frame.PrintContentPlain();
                    break;
                case "save":
                    frame.SaveContentPlain();
                    break;
                case "helpAbout":
                    {
                        HelpAboutPanelController hpc = new HelpAboutPanelController(new HelpAboutPanel());
                        hpc.ViewContainer = ViewContainer;
                        ViewContainer.AddControl(hpc, GVControlAlign.Center);
                    }
                    break;
                case "windowOpen":
                    FrameMain frame1 = new FrameMain();
                    frame1.Icon = Properties.Resources.GCalIcon2;
                    frame1.Text = "GCAL (secondary)";
                    frame1.Show();
                    break;
                case "windowClose":
                    frame.Close();
                    break;
                case "getCurrentContent":
                    return currentObject;
                default:
                    break;
            }

            return GSCore.Void;
        }

    }
}
