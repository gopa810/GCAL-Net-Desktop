using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;
using GCAL.CompositeViews;

namespace GCAL.CompositeViews
{
    public partial class AppDayTab : UserControl
    {
        private TResultApp m_appday;
        private int p_mode = 0;

        public AppDayTabController Controller { get; set; }
        public GregorianDateTime appDayDateTime = new GregorianDateTime();
        public GCLocation appDayLocation = null;

        public AppDayTab()
        {
            InitializeComponent();
            richTextBox1.Dock = DockStyle.Fill;
            pictureBox1.Dock = DockStyle.Fill;

            string s = Properties.Settings.Default.AppDayDateTime;
            appDayDateTime.EncodedString = Properties.Settings.Default.AppDayDateTime;
            s = Properties.Settings.Default.AppDayLocation;
            if (s.Length < 1)
                s = GCGlobal.LastLocation.EncodedString;
            appDayLocation = new GCLocation();
            appDayLocation.EncodedString = s;
            SetMode(Properties.Settings.Default.AppDayShowMode);
            Recalculate();
        }
        public TResultBase getCurrentContent()
        {
            return m_appday;
        }


        public void SetMode(int i)
        {
            if (i == -1)
            {
                richTextBox1.Visible = false;
                pictureBox1.Visible = true;
            }
            else if (i == 0)
            {
                richTextBox1.Visible = true;
                pictureBox1.Visible = false;

                p_mode = i;
                DisplayCalendarResult();
                Properties.Settings.Default.AppDayShowMode = i;
                Properties.Settings.Default.Save();
            }
            else if (i == 1)
            {
                richTextBox1.Visible = true;
                pictureBox1.Visible = false;

                p_mode = i;
                DisplayCalendarResult();
                Properties.Settings.Default.AppDayShowMode = i;
                Properties.Settings.Default.Save();
            }

        }

        public string LocationText(string s)
        {
            if (s != null)
            {
                toolStripButton1.Text = s;
            }
            return toolStripButton1.Text;
        }
        public string StartDateText(string s)
        {
            if (s != null)
            {
                toolStripButton2.Text = s;
            }
            return toolStripButton2.Text;
        }
        public string StartTimeText(string s)
        {
            if (s != null)
            {
                toolStripButton3.Text = s;
            }
            return toolStripButton3.Text;
        }

        public void Recalculate()
        {
            bool settingsChanged = false;
            string s = appDayDateTime.EncodedString;
            if (!s.Equals(Properties.Settings.Default.AppDayDateTime))
            {
                Properties.Settings.Default.AppDayDateTime = s;
                settingsChanged = true;
            }
            s = appDayLocation.EncodedString;
            if (!s.Equals(Properties.Settings.Default.AppDayLocation))
            {
                Properties.Settings.Default.AppDayLocation = s;
                settingsChanged = true;
            }
            if (settingsChanged)
            {
                Properties.Settings.Default.Save();
            }

            LocationText(appDayLocation.Title);
            StartDateText(appDayDateTime.ToString());
            StartTimeText(appDayDateTime.ShortTimeString());

            if (p_mode == 0 || p_mode == 1)
            {
                m_appday = new TResultApp();
                m_appday.calculateAppDay(appDayLocation, appDayDateTime);
                DisplayCalendarResult();
            }
        }

        public void DisplayCalendarResult()
        {
            if (p_mode == 0)
            {
                if (m_appday == null)
                    Recalculate();
                else
                    richTextBox1.Text = m_appday.formatText(GCDataFormat.PlainText);
            }
            else if (p_mode == 1)
            {
                if (m_appday == null)
                    Recalculate();
                else
                    richTextBox1.Rtf = m_appday.formatText(GCDataFormat.Rtf);
            }
        }

        private void onLocationClick(object sender, EventArgs e)
        {
            SelectLocationInputPanel d = new SelectLocationInputPanel();
            d.OnLocationSelected += new TBButtonPressed(onLocationDone);
            SelectLocationInputPanelController dc = new SelectLocationInputPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void onLocationDone(object sender, EventArgs e)
        {
            if (sender is GCLocation)
            {
                GCLocation lr = sender as GCLocation;
                GCGlobal.AddRecentLocation(lr);
                appDayLocation = lr;
                Recalculate();
            }
        }

        private void onDateRangeClick(object sender, EventArgs e)
        {
            DateTimePanel d = new DateTimePanel();
            d.OnDateTimeSelected += new TBButtonPressed(onDateRangeCallback);
            d.CustomTitle = "Event Date Time";
            d.DateTime = appDayDateTime;
            DateTimePanelController dlg16 = new DateTimePanelController(d);
            dlg16.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void onDateRangeCallback(object sender, EventArgs e)
        {
            if (sender is GregorianDateTime)
            {
                GregorianDateTime gdt = sender as GregorianDateTime;
                appDayDateTime = gdt;
                StartDateText(gdt.ToString());
                StartTimeText(gdt.ShortTimeString());
                Recalculate();
            }
        }

        private void toolStripDropDownButton2_DropDownOpening(object sender, EventArgs e)
        {
            plainTextToolStripMenuItem.Checked = (p_mode == 0);
            richTextToolStripMenuItem.Checked = (p_mode == 1);

            smallTextToolStripMenuItem.Enabled = (p_mode != 2);
            smallTextToolStripMenuItem1.Enabled = (p_mode != 2);
            normalTextToolStripMenuItem.Enabled = (p_mode != 2);
            largestTextToolStripMenuItem.Enabled = (p_mode != 2);
            largeTextToolStripMenuItem.Enabled = (p_mode != 2);

            smallTextToolStripMenuItem.Checked = (GCLayoutData.LayoutSizeIndex == 0);
            smallTextToolStripMenuItem1.Checked = (GCLayoutData.LayoutSizeIndex == 1);
            normalTextToolStripMenuItem.Checked = (GCLayoutData.LayoutSizeIndex == 2);
            largeTextToolStripMenuItem.Checked = (GCLayoutData.LayoutSizeIndex == 3);
            largestTextToolStripMenuItem.Checked = (GCLayoutData.LayoutSizeIndex == 4);

        }

        private void plainTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMode(0);
        }

        private void richTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMode(1);
        }

        private void smallTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 0;
            DisplayCalendarResult();
        }

        private void smallTextToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 1;
            DisplayCalendarResult();
        }

        private void normalTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 2;
            DisplayCalendarResult();
        }

        private void largeTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 3;
            DisplayCalendarResult();
        }

        private void largestTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 4;
            DisplayCalendarResult();
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrameMain.SaveContentPlain(m_appday);
        }

        private void printToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            if (Controller != null)
                Controller.ExecuteMessage("printContent", m_appday);
        }

        private void AppDayTab_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void AppDayTab_Enter(object sender, EventArgs e)
        {

        }



    }
}
