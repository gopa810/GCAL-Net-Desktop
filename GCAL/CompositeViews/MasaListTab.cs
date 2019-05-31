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
    public partial class MasaListTab : UserControl
    {
        private TResultMasaList m_masas;
        public MasaListTabController Controller { get; set; }
        private int p_mode = 0;

        public GCLocation masaLocation = null;
        public int masaYear = 0;
        public int masaCount = 0;

        public MasaListTab()
        {
            InitializeComponent();
            richTextBox1.Dock = DockStyle.Fill;
            pictureBox1.Dock = DockStyle.Fill;
            toolStripTextBox1.Text = DateTime.Now.Year.ToString();
            toolStripTextBox2.Text = "1";

            string s = Properties.Settings.Default.MasaListLocation;
            if (s.Length < 1)
                s = GCGlobal.LastLocation.EncodedString;
            masaLocation = new GCLocation();
            masaLocation.EncodedString = s;
            masaYear = Properties.Settings.Default.MasaListYear;
            if (masaYear < 1600)
                masaYear = DateTime.Now.Year;
            masaCount = Properties.Settings.Default.MasaListCount;
            if (masaCount < 1)
                masaCount = 1;

            SetMode(Properties.Settings.Default.MasaListShowMode);
        }

        public TResultBase getCurrentContent()
        {
            return m_masas;
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
                Properties.Settings.Default.MasaListShowMode = i;
                Properties.Settings.Default.Save();
            }
            else if (i == 1)
            {
                richTextBox1.Visible = true;
                pictureBox1.Visible = false;

                p_mode = i;
                DisplayCalendarResult();
                Properties.Settings.Default.MasaListShowMode = i;
                Properties.Settings.Default.Save();
            }

        }

        public void Recalculate()
        {
            bool settingsChanged = false;
            string s = masaLocation.EncodedString;
            if (!s.Equals(Properties.Settings.Default.MasaListLocation))
            {
                Properties.Settings.Default.MasaListLocation = s;
                settingsChanged = true;
            }
            if (masaYear != Properties.Settings.Default.MasaListYear)
            {
                Properties.Settings.Default.MasaListYear = masaYear;
                settingsChanged = true;
            }
            if (masaCount != Properties.Settings.Default.MasaListCount)
            {
                Properties.Settings.Default.MasaListCount = masaCount;
                settingsChanged = true;
            }
            if (settingsChanged)
            {
                Properties.Settings.Default.Save();
            }

            if (p_mode == 0 || p_mode == 1)
            {
                m_masas = new TResultMasaList();
                m_masas.CalculateMasaList(masaLocation, masaYear, masaCount);
                DisplayCalendarResult();
            }
        }

        public void DisplayCalendarResult()
        {
            if (p_mode == 0)
            {
                if (m_masas == null)
                    Recalculate();
                else
                    richTextBox1.Text = m_masas.formatText(GCDataFormat.PlainText);
            }
            else if (p_mode == 1)
            {
                if (m_masas == null)
                    Recalculate();
                else
                    richTextBox1.Rtf = m_masas.formatText(GCDataFormat.Rtf);
            }
        }

        public int SelectedYear
        {
            get
            {
                int a = 0;
                if (int.TryParse(toolStripTextBox1.Text, out a))
                {
                    if (a < 1800)
                        a = 1800;
                    if (a > 2200)
                        a = 2200;
                }
                else
                {
                    a = DateTime.Now.Year;
                }
                return a;
            }
        }

        public int SelectedCount
        {
            get
            {
                int a = 0;
                if (int.TryParse(toolStripTextBox2.Text, out a))
                {
                    if (a < 1)
                        a = 1;
                    if (a > 30)
                        a = 30;
                }
                else
                {
                    a = 1;
                }
                return a;
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
                masaLocation = lr;
                Recalculate();
            }
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrameMain.SaveContentPlain(m_masas);
        }

        private void printToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Controller != null)
                Controller.ExecuteMessage("printContent", m_masas);
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

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_Leave(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            masaYear = SelectedYear;
            masaCount = SelectedCount;
            toolStripTextBox1.Text = masaYear.ToString();
            toolStripTextBox2.Text = masaCount.ToString();
            Recalculate();
        }

        public string LocationText(string s)
        {
            if (s != null)
            {
                toolStripButton1.Text = s;
            }
            return toolStripButton1.Text;
        }
    }
}
