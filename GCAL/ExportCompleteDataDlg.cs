using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using GCAL.Base;

namespace GCAL
{
    public partial class ExportCompleteDataDlg : Form
    {
        public ExportCompleteDataDlg()
        {
            InitializeComponent();
            numericUpDown1.Minimum = 1900;
            numericUpDown1.Maximum = 2150;
            numericUpDown1.Value = DateTime.Now.Year;
            numericUpDown2.Value = DateTime.Now.Year;
            numericUpDown2.Minimum = DateTime.Now.Year;
            numericUpDown2.Maximum = 2200;
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            textBox2.Text = Properties.Settings.Default.OutputDirCompleteData;
            if (textBox2.Text.Length == 0)
                textBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Minimum < numericUpDown1.Value)
            {
                numericUpDown2.Minimum = numericUpDown1.Value;
                if (numericUpDown2.Value < numericUpDown1.Value)
                    numericUpDown2.Value = numericUpDown1.Value;
            }
        }

        public class LocProxy
        {
            public TLocation loc;
            public LocProxy(TLocation l)
            {
                loc = l;
            }
            public override string ToString()
            {
                return string.Format("{0} [{1}]", loc.CityName, loc.Country.Name);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            int i = 0;

            listBox2.BeginUpdate();
            listBox2.Items.Clear();

            foreach (TLocation loc in TLocationDatabase.LocationList)
            {
                if (loc.CityName.IndexOf(str, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    listBox2.Items.Add(new LocProxy(loc));
                    i++;
                }
                if (i > 100)
                    break;
            }

            listBox2.EndUpdate();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0 && listBox2.SelectedIndex < listBox2.Items.Count)
            {
                listBox1.Items.Add(listBox2.Items[listBox2.SelectedIndex]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        public List<TLocation> SelectedLocations = new List<TLocation>();
        public int StartYear = 2020;
        public int EndYear = 2020;
        public string SelectedDirectory = "";
        public bool includeSun = false;
        public bool includeCore = false;
        public bool isPdf = false;

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (object obj in listBox1.Items)
            {
                SelectedLocations.Add(((LocProxy)obj).loc);
            }
            StartYear = (int)numericUpDown1.Value;
            EndYear = (int)numericUpDown2.Value;
            SelectedDirectory = textBox2.Text;
            includeSun = checkBox1.Checked;
            includeCore = checkBox2.Checked;

            Properties.Settings.Default.OutputDirCompleteData = SelectedDirectory;
            Properties.Settings.Default.Save();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.SelectedPath = textBox2.Text;

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = fd.SelectedPath;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string s in File.ReadAllLines(ofd.FileName))
                {
                    bool isok;
                    double d;
                    string[] p = s.Split(',');
                    if (p.Length == 5)
                    {
                        TLocation loc = new TLocation();
                        loc.CityName = p[0];
                        loc.CountryISOCode = p[1];
                        isok = GCEarthData.ToDouble(p[2], out d, 'N', 'S');
                        if (!isok) continue;
                        loc.Latitude = d;
                        isok = GCEarthData.ToDouble(p[3], out d, 'E', 'W');
                        if (!isok) continue;
                        loc.Longitude = d;
                        loc.TimeZoneName = p[4];

                        listBox1.Items.Add(new LocProxy(loc));
                    }
                }
            }
        }
    }
}
