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
    public partial class LocationChoosePanel : UserControl
    {
        public static int nCurrentCountry = 0;

        public static TLocation lastLocation = null;

        public ChooseLocationPanelController Controller { get; set; }

        public event TBButtonPressed OnButtonYes;

        public event TBButtonPressed OnLocationSelected;

        public LocationChoosePanel()
        {
            InitializeComponent();

            InitCountries();

            comboBoxCountry.SelectedIndex = nCurrentCountry;

            SelectLocation(lastLocation);
        }

        public TLocation SelectedLocation
        {
            get
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    lastLocation = listView1.SelectedItems[0].Tag as TLocation;
                    return lastLocation;
                }

                if (lastLocation != null)
                    return lastLocation;

                if (TLocationDatabase.LocationList != null)
                    return TLocationDatabase.LocationList[0];

                return null;
            }
            set
            {
                SelectLocation(value);
            }
        }

        private void SelectLocation(TLocation loc)
        {
            if (loc == null)
            {
                SelectFirstLocation();
                return;
            }

            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.Tag == loc)
                {
                    listView1.SelectedItems.Clear();
                    lvi.Selected = true;
                    listView1.EnsureVisible(lvi.Index);
                    return;
                }
            }

            SelectFirstLocation();
        }

        private void InitCountries()
        {
            comboBoxCountry.BeginUpdate();
            foreach (TCountry tc in TCountry.Countries)
            {
                comboBoxCountry.Items.Add(tc);
            }
            comboBoxCountry.EndUpdate();
            comboBoxCountry.Items.Insert(0, "<All Countries>");
        }

        private void comboBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListViewWithLocations();
            SelectLocation(lastLocation);
        }

        private void AddLocationToListView(TLocation loc)
        {
            ListViewItem lvi = new ListViewItem(loc.CityName);
            lvi.SubItems.Add(loc.Country.Name);
            lvi.SubItems.Add(GCEarthData.GetTextLatitude(loc.Latitude));
            lvi.SubItems.Add(GCEarthData.GetTextLongitude(loc.Longitude));
            lvi.SubItems.Add(TTimeZone.GetTimeZoneOffsetText(loc.TimeZone.OffsetMinutes/60.0));
            lvi.Tag = loc;
            listView1.Items.Add(lvi);
        }

        private void SelectFirstLocation()
        {
            if (listView1.Items.Count > 0)
            {
                listView1.SelectedItems.Clear();
                listView1.Items[0].Selected = true;
                listView1.EnsureVisible(0);
            }
        }

        private void buttonSetAllCountries_Click(object sender, EventArgs e)
        {
            comboBoxCountry.SelectedIndex = 0;
        }

        private void buttonSetCountryByCity_Click(object sender, EventArgs e)
        {
            if (lastLocation != null)
            {
                TLocation loc = lastLocation;
                int i = 0;
                foreach (object c in comboBoxCountry.Items)
                {
                    if (c is TCountry)
                    {
                        TCountry tc = c as TCountry;
                        if (tc.ISOCode.Equals(loc.CountryISOCode))
                        {
                            comboBoxCountry.SelectedIndex = i;
                            break;
                        }
                    }
                    i++;
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                lastLocation = listView1.SelectedItems[0].Tag as TLocation;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateListViewWithLocations();
            SelectFirstLocation();

            /*foreach (ListViewItem lvi in listView1.Items)
            {
                TLocation L = lvi.Tag as TLocation;
                if (L == null)
                    continue;

                if (L.CityName.IndexOf(s, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    listView1.EnsureVisible(lvi.Index);
                    lvi.Selected = true;
                    break;
                }
            }*/
        }

        private void UpdateListViewWithLocations()
        {
            string s = textBox1.Text.Trim();
            TCountry tc = null;
            if (comboBoxCountry.SelectedItem is TCountry)
            {
                tc = comboBoxCountry.SelectedItem as TCountry;
            }

            listView1.BeginUpdate();
            listView1.Items.Clear();
            int limit = 100;
            int added = 0;
            int lastPart = 0;
            List<TLocation>[] p = {
                                      new List<TLocation>(),
                                      new List<TLocation>(),
                                      new List<TLocation>()
                                  };
            foreach (TLocation loc in TLocationDatabase.LocationList)
            {
                if ((tc == null || loc.CountryISOCode.Equals(tc.ISOCode))
                    && (s.Length == 0 || (lastPart = loc.ContainsSubstring(s)) > 0))
                {
                    switch (lastPart)
                    {
                        case 1:
                            p[0].Add(loc);
                            break;
                        case 2:
                            p[1].Add(loc);
                            break;
                        default:
                            p[2].Add(loc);
                            break;
                    }
                    added++;
                }
                if (added > limit)
                    break;
            }
            for (int j = 0; j < 3; j++)
            {
                foreach (TLocation loc in p[j])
                {
                    AddLocationToListView(loc);
                }
                p[j].Clear();
            }
            listView1.EndUpdate();

            if (added > limit)
            {
                labelLimitInfo.Text = string.Format("Number of results limited to {0}", added);
                labelLimitInfo.ForeColor = Color.DarkRed;
            }
            else
            {
                labelLimitInfo.Text = string.Format("Number of results: {0}", added);
                labelLimitInfo.ForeColor = Color.DarkGreen;
            }

        }

        public bool ButtonOkEnable
        {
            get
            {
                return buttonOK.Visible;
            }
            set
            {
                buttonOK.Visible = value;
            }
        }

        public bool ButtonCancelEnable
        {
            get
            {
                return buttonCancel.Visible;
            }
            set
            {
                buttonCancel.Visible = value;
            }
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (OnButtonYes != null)
                OnButtonYes(this, e);

            if (OnLocationSelected != null)
                OnLocationSelected(SelectedLocation.GetLocationRef(), e);

            Controller.RemoveFromContainer();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }
    }

    public class ChooseLocationPanelController : GVCore
    {
        public ChooseLocationPanelController(LocationChoosePanel v)
        {
            v.Controller = this;
            View = v;
            v.ButtonCancelEnable = true;
            v.ButtonOkEnable = true;
        }
    }
}
