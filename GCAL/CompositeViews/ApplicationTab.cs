using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base.Scripting;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public partial class ApplicationTab : UserControl
    {
        public ApplicationTab()
        {
            InitializeComponent();

            gvListBanner1.AddTab("File", null);
            gvListBanner1.AddTab("Save As", "save");
            gvListBanner1.AddTab("Print", "print");
            gvListBanner1.AddTab("Help", null);
            gvListBanner1.AddTab("About", "helpAbout");
            gvListBanner1.AddTab("Help", "help");
            gvListBanner1.AddTab("Tip of the Day", "showTipOfTheDay");
            gvListBanner1.AddTab("Organizer", null);
            gvListBanner1.AddTab("Locations", "locs");
            gvListBanner1.AddTab("Festival Books", "festivalBooks");
            gvListBanner1.AddTab("Festivals", "events");
            gvListBanner1.AddTab("Countries", "cntr");
            gvListBanner1.AddTab("Timezones", "tzones");
            gvListBanner1.AddTab("Display Settings", null);
            gvListBanner1.AddTab("General", "dispSetGeneral");
            gvListBanner1.AddTab("Calendar", "dispSetCalendar");
            gvListBanner1.AddTab("Core Events", "dispSetCoreEvents");
            gvListBanner1.AddTab("Today", "dispSetToday");
            gvListBanner1.AddTab("Appearance Day", "dispSetAppDay");
            gvListBanner1.AddTab("Window", null);
            gvListBanner1.AddTab("New", "windowOpen");
            gvListBanner1.AddTab("Close", "windowClose");

            gvListBanner1.Controller = new GVListBannerController(gvListBanner1);
        }

        public GVControlContainer ViewContainer
        {
            get
            {
                return gvControlContainer1;
            }
        }

        private GVCore p_delegate = null;
        public GVCore Controller
        {
            get { return p_delegate; }
            set
            {
                p_delegate = value;
                gvListBanner1.Controller.Parent = value;
            }
        }

        public int SelectedIndex
        {
            get 
            {
                return gvListBanner1.SelectedIndex; 
            }
            set
            {
                gvListBanner1.SelectedIndex = value;
            }
        }
        public int SelectedIndexNoResponse
        {
            get
            {
                return gvListBanner1.SelectedIndexNoResponse;
            }
            set
            {
                gvListBanner1.SelectedIndexNoResponse = value;
            }
        }

    }

}
