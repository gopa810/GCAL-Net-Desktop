using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using GCAL.Base;
using GCAL.Views;
using GCAL.Base.Scripting;

namespace GCAL
{
    public partial class FrameMain : Form
    {
        public FrameMainController frameDelegate;

        public Font printFont;
        public string[] stringToPrint;
        public int printLineCurr;

        public FrameMain()
        {
            InitializeComponent();

            Properties.Settings.Default.CoreAstroSystem = (int)GCCoreAstronomy.AstronomySystem.Meeus;
            GCCoreAstronomy.System = (GCCoreAstronomy.AstronomySystem)Properties.Settings.Default.CoreAstroSystem;

            frameDelegate = new FrameMainController(this);
            if (GCUserInterface.windowController == null)
                GCUserInterface.windowController = frameDelegate;

            // setup of tab bar
            gvTabBanner1.AddTab("Application", "app");
            gvTabBanner1.AddTab("Search", "search");
            gvTabBanner1.AddTab("Today", "today");
            gvTabBanner1.AddTab("Calendar", "cal");
            gvTabBanner1.AddTab("Core Events", "events");
            gvTabBanner1.AddTab("Appearance Day", "appday");
            gvTabBanner1.AddTab("Masa List", "masalist");
            gvTabBanner1.AddTab("Rated Events", "ratedevents");

            gvTabBanner1.Controller = new GVTabBannerController(gvTabBanner1);
            gvTabBanner1.Controller.Parent = frameDelegate;
            gvTabBanner1.SelectTab("today");

            frameDelegate.ViewContainer = gvTabBanner1;
            frameDelegate.ExecuteMessage("showTipAtStartup");

            printFont = new Font("Lucida Console", 10);



            GCAL.Base.VSOP87.Testing.Test();

            /*string p = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (p.IndexOf("Siddhanta") >= 0)
            {
                Properties.Settings.Default.CoreAstroSystem = (int)GCCoreAstronomy.AstronomySystem.SuryaSiddhanta;
            }*/
            RefreshTitleText();

            //GCFestivalBookCollection.Export(@"e:\temp\events.js", 15);
            /*using (StreamWriter sw = new StreamWriter("e:\\temp\\tz.js"))
            {
                foreach (TTimeZone tz in TTimeZone.TimeZoneList)
                {
                    sw.WriteLine(tz.JsonString + ",");
                }
            }*/
        }

        private string GetTimeStr(double d)
        {
            int secs = Convert.ToInt32(d * 3600);
            int s = secs % 60;
            secs /= 60;
            int m = secs % 60;
            secs /= 60;
            return string.Format("{0:00}:{1:00}:{2:00}", secs, m, s);
        }

        public GVTabBanner TabBanner
        {
            get
            {
                return gvTabBanner1;
            }
        }

        public void SaveContentPlain()
        {
            TResultBase current = getCurrentObject();
            if (current == null)
                return;
            SaveContentPlain(current);
        }

        public static void SaveContentPlain(TResultBase current)
        {
            if (current == null)
                return;

            TResultFormatCollection formats = current.getFormats();

            SaveFileDialog fd = new SaveFileDialog();
            fd.FileName = formats.ResultName;
            fd.OverwritePrompt = true;
            fd.AddExtension = true;
            fd.Filter = formats.getDialogFilterString();
            fd.FilterIndex = 1;

            StringBuilder sb = new StringBuilder();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (formats.Formats.Count >= fd.FilterIndex && fd.FilterIndex > 0)
                {
                    string result = current.formatText(formats.Formats[fd.FilterIndex - 1].TemplateName);
                    File.WriteAllText(fd.FileName, result);
                }
            }

        }

        public TResultBase getCurrentObject()
        {
            return frameDelegate.getCurrentObject();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintContentPlain();
        }

        public void PrintContentPlain()
        {
            PrintContentPlain(getCurrentObject());
        }

        public void PrintContentPlain(TResultBase current)
        {
            PrintDialog pd = new PrintDialog();

            if (pd.ShowDialog() != DialogResult.OK)
                return;

            stringToPrint = current.formatText(GCDataFormat.PlainText).Split('\r', ' ');
            printLineCurr = 0;
            printDocument1.PrinterSettings = pd.PrinterSettings;
            printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Iterate over the file, printing each line.
            while (count < linesPerPage && printLineCurr < stringToPrint.Length)
            {
                line = stringToPrint[printLineCurr];
                printLineCurr++;

                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (printLineCurr < stringToPrint.Length)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }


        public void RefreshTitleText()
        {
            switch (GCCoreAstronomy.System)
            {
                case GCCoreAstronomy.AstronomySystem.Meeus:
                    this.Text = "Gaurabda Calendar";
                    break;
            }
        }
    }
}
