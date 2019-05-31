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


namespace GCAL.CompositeViews
{
    public partial class TimezonesPanel : UserControl
    {
        public TimezonesPanelDelegate Controller { get; set; }

        public TimezonesPanel()
        {
            InitializeComponent();


            UpdateTimezoneList();

        }


        public void UpdateTimezoneList()
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (TTimeZone timezone in TTimeZone.TimeZoneList)
            {
                ListViewItem lvi = new ListViewItem("");
                UpdateListViewItem(timezone, lvi);
                listView1.Items.Add(lvi);
            }
            listView1.EndUpdate();
        }

        private static void UpdateListViewItem(TTimeZone timezone, ListViewItem lvi)
        {
            lvi.SubItems.Clear();
            lvi.Text = timezone.Name;
            lvi.SubItems.Add(TTimeZone.GetTimeZoneOffsetText(timezone.OffsetMinutes / 60.0));
            lvi.Tag = timezone;
        }

        private TTimeZone SelectedTimeZone
        {
            get
            {
                if (listView1.SelectedItems.Count == 0)
                    return null;

                return listView1.SelectedItems[0].Tag as TTimeZone;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt||";
            sfd.DefaultExt = ".txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                TTimeZone.SaveFile(sfd.FileName);
            }
        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim().Length == 0)
            {
                UpdateTimezoneList();
                return;
            }

            string[] ps = textBox2.Text.Trim().ToLower().Split(' ');
            int A, B;

            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (TTimeZone timezone in TTimeZone.TimeZoneList)
            {
                A = B = 0;
                foreach (string s in ps)
                {
                    A++;
                    if (timezone.Name.IndexOf(s, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        B++;
                }
                if (A == B)
                {
                    ListViewItem lvi = new ListViewItem(timezone.Name);
                    lvi.SubItems.Add(TTimeZone.GetTimeZoneOffsetText(timezone.OffsetMinutes / 60.0));
                    lvi.Tag = timezone;
                    listView1.Items.Add(lvi);
                }
            }
            listView1.EndUpdate();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (Controller.ViewContainer != null)
            {
                TimezoneDetails d = new TimezoneDetails();
                d.OnButtonSave += new TBButtonPressed(OnDetailsDialogSaved);
                d.setTimeZone(null);
                TimezoneDetailsController dc = new TimezoneDetailsController(d);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (Controller.ViewContainer != null)
            {
                TimezoneDetails d = new TimezoneDetails();
                d.setTimeZone(SelectedTimeZone);
                d.OnButtonSave += new TBButtonPressed(OnDetailsDialogSaved);
                TimezoneDetailsController dc = new TimezoneDetailsController(d);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (Controller.ViewContainer != null)
            {
                TimezoneDetails d = new TimezoneDetails();
                d.setCopyTimeZone(SelectedTimeZone);
                d.OnButtonSave += new TBButtonPressed(OnDetailsDialogSaved);
                TimezoneDetailsController dc = new TimezoneDetailsController(d);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        private void OnDetailsDialogSaved(object sender, EventArgs e)
        {
            TTimeZone tz = SelectedTimeZone;
            if (tz == null)
            {
                UpdateTimezoneList();
            }
            else
            {
                foreach (ListViewItem lvi in listView1.Items)
                {
                    if (lvi.Tag == tz)
                    {
                        UpdateListViewItem(tz, lvi);
                        break;
                    }
                }
            }
        }

    }

    public class TimezonesPanelDelegate : GVCore
    {
        public TimezonesPanelDelegate(TimezonesPanel v)
        {
            View = v;
            v.Controller = this;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            return base.ExecuteMessage(token, args);
        }
    }

}
