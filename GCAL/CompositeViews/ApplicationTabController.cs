using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public class ApplicationTabController : GVCore
    {
        private DispSetGeneralDelegate dsGeneral = null;
        private DispSetAppDayDelegate dsAppDay = null;
        private DispSetCalendarDelegate dsCalendar = null;
        private DispSetCoreEventsDelegate dsCoreEvents = null;
        private DispSetTodayDelegate dsToday = null;
        private EventsPanelDelegate evPanel = null;
        private LocationsPanelController locPanel = null;
        private CountriesPanelDelegate couPanel = null;
        private TimezonesPanelDelegate timPanel = null;
        private FestivalBooksPanelDelegate festPanel = null;
        private HelpPanelController helpPanel = null;

        private GVCore currentPanelDisplayed = null;

        public ApplicationTabController(ApplicationTab view)
        {
            View = view;
            view.Controller = this;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            if (token.Equals("setTabChanged"))
            {
                token = GVListBanner.MsgListItemChanged;
            }

            if (token.Equals(GVListBanner.MsgListItemChanged))
            {
                GSCore a1 = args.getSafe(0);
                switch (a1.getStringValue())
                {
                    case "dispSetGeneral":
                        if (dsGeneral == null)
                            dsGeneral = new DispSetGeneralDelegate(new DispSetGeneral());
                        currentPanelDisplayed = dsGeneral;
                        ShowPanel(dsGeneral, GVControlAlign.Scroll);
                        break;
                    case "dispSetToday":
                        if (dsToday == null)
                            dsToday = new DispSetTodayDelegate(new DispSetToday());
                        currentPanelDisplayed = dsToday;
                        ShowPanel(dsToday, GVControlAlign.Scroll);
                        break;
                    case "dispSetCalendar":
                        if (dsCalendar == null)
                            dsCalendar = new DispSetCalendarDelegate(new DispSetCalendar());
                        currentPanelDisplayed = dsCalendar;
                        ShowPanel(dsCalendar, GVControlAlign.Scroll);
                        break;
                    case "dispSetCoreEvents":
                        if (dsCoreEvents == null)
                            dsCoreEvents = new DispSetCoreEventsDelegate(new DispSetCoreEvents());
                        currentPanelDisplayed = dsCoreEvents;
                        ShowPanel(dsCoreEvents, GVControlAlign.Scroll);
                        break;
                    case "dispSetAppDay":
                        if (dsAppDay == null)
                            dsAppDay = new DispSetAppDayDelegate(new DispSetAppDay());
                        currentPanelDisplayed = dsAppDay;
                        ShowPanel(dsAppDay, GVControlAlign.Scroll);
                        break;
                    case "locs":
                        if (locPanel == null)
                            locPanel = new LocationsPanelController(new LocationsPanel());
                        currentPanelDisplayed = locPanel;
                        locPanel.ViewContainer = getView().ViewContainer;
                        ShowPanel(locPanel, GVControlAlign.Fill);
                        break;
                    case "events":
                        if (evPanel == null)
                        {
                            EventsPanel ep = new EventsPanel();
                            evPanel = new EventsPanelDelegate(ep);
                            evPanel.ViewContainer = getView().ViewContainer;
                        }
                        currentPanelDisplayed = evPanel;
                        ShowPanel(evPanel, GVControlAlign.Fill);
                        break;
                    case "cntr":
                        if (couPanel == null)
                        {
                            CountriesPanel cp = new CountriesPanel();
                            cp.ViewContainer = getView().ViewContainer;
                            couPanel = new CountriesPanelDelegate(cp);
                        }
                        currentPanelDisplayed = couPanel;
                        ShowPanel(couPanel, GVControlAlign.Fill);
                        break;
                    case "tzones":
                        if (timPanel == null)
                        {
                            timPanel = new TimezonesPanelDelegate(new TimezonesPanel());
                            timPanel.ViewContainer = getView().ViewContainer;
                        }
                        currentPanelDisplayed = timPanel;
                        ShowPanel(timPanel, GVControlAlign.Fill);
                        break;
                    case "festivalBooks":
                        if (festPanel == null)
                        {
                            festPanel = new FestivalBooksPanelDelegate(new FestivalBooksPanel());
                            festPanel.ViewContainer = getView().ViewContainer;
                        }
                        currentPanelDisplayed = festPanel;
                        ShowPanel(festPanel, GVControlAlign.Fill);
                        break;
                    case "save":
                    case "print":
                    case "showTipOfTheDay":
                    case "helpAbout":
                    case "windowOpen":
                    case "windowClose":
                        if (Parent != null)
                        {
                            getView().SelectedIndexNoResponse = -1;
                            Parent.ExecuteMessage(a1.getStringValue());
                        }
                        break;
                    case "help":
                        if (helpPanel == null)
                            helpPanel = new HelpPanelController(new HelpPanel());
                        ShowPanel(helpPanel, GVControlAlign.Fill);
                        helpPanel.ShowRichText(Properties.Resources.gcal_help);
                        break;
                    default:
                        break;
                }
            }
            else if (token.Equals("currExec"))
            {
                if (currentPanelDisplayed != null)
                {
                    currentPanelDisplayed.ExecuteMessage(args.getSafe(0).getStringValue(), args.getSublist(1));
                }
            }
            else if (token.Equals("saveSettings"))
            {
                if (currentPanelDisplayed != null)
                {
                    currentPanelDisplayed.ExecuteMessage(GVControlContainer.MsgViewWillHide);
                }
            }
            else
            {
                return base.ExecuteMessage(token, args);
            }

            return GSCore.Void;
        }

        public ApplicationTab getView()
        {
            return View as ApplicationTab;
        }

        /// <summary>
        /// Displays user control in the area reserved for user content
        /// </summary>
        /// <param name="userControl"></param>
        public void ShowPanel(GVCore userControl, GVControlAlign align)
        {
            GVControlContainer container = (View as ApplicationTab).ViewContainer;

            userControl.Parent = this;
            container.RemoveAll();
            container.AddControl(userControl, align);
        }
    }

}
