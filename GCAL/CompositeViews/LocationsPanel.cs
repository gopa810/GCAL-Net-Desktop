using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using GCAL.Base;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public partial class LocationsPanel : UserControl
    {
        public GVCore Controller { get; set; }

        public LocationsPanel()
        {
            InitializeComponent();

            InitCountries();

            m_wndCountry.SelectedIndex = 0;
        }

        public void InitCountries()
        {
            int i, m, a;

            //	m_wndCtrs.DeleteAllItems();
            m_wndCountry.BeginUpdate();
            m_wndCountry.Items.Clear();
            foreach (TCountry country in TCountry.Countries)
            {
                a = m_wndCountry.Items.Add(country);
            }
            m_wndCountry.Sorted = true;
            m_wndCountry.EndUpdate();
            m_wndCountry.Sorted = false;
            m_wndCountry.Items.Insert(0, "<all countries>");

        }

        private void m_wndCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLocationList();
        }

        /// <summary>
        /// Filling ListView with cities
        /// </summary>
        private void UpdateLocationList()
        {
            string sp = textBox1.Text.Trim();
            bool emptySp = (sp.Length == 0);
            object obj = m_wndCountry.SelectedItem;
            TCountry country = (obj is TCountry) ? (obj as TCountry) : null;
            int A = 0, B = 100;
            int lastCompare = 2;

            listViewLocations.BeginUpdate();
            listViewLocations.Items.Clear();
            List<TLocation>[] p = {
                                      new List<TLocation>(),
                                      new List<TLocation>(),
                                      new List<TLocation>()
                                  };
            foreach (TLocation L in TLocationDatabase.LocationList)
            {
                if (country == null || L.CountryISOCode.Equals(country.ISOCode))
                {
                    if (emptySp || (lastCompare = L.ContainsSubstring(sp)) > 0)
                    {
                        switch (lastCompare)
                        {
                            case 1:
                                p[0].Add(L);
                                break;
                            case 2:
                                p[1].Add(L);
                                break;
                            default:
                                p[2].Add(L);
                                break;
                        }
                        A++;
                    }
                }

                if (A > B)
                {
                    break;
                }
            }
            for (int j = 0; j < 3; j++)
            {
                foreach (TLocation L in p[j])
                {
                    listViewLocations.Items.Add(ListItemFromLocation(L));
                }
                p[j].Clear();
            }
            listViewLocations.EndUpdate();

            if (A > B)
            {
                labelLimitInfo.Text = string.Format("Number of results limited to {0}", A);
                labelLimitInfo.ForeColor = Color.DarkRed;
            }
            else
            {
                labelLimitInfo.Text = string.Format("Number of results: {0}", A);
                labelLimitInfo.ForeColor = Color.DarkGreen;
            }
        }

        private ListViewItem ListItemFromLocation(TLocation L)
        {
            ListViewItem lvi = new ListViewItem(L.CityName);
            lvi.SubItems.Add(L.Country.Name);
            lvi.SubItems.Add(GCEarthData.GetTextLatitude(L.Latitude));
            lvi.SubItems.Add(GCEarthData.GetTextLongitude(L.Longitude));
            lvi.SubItems.Add(L.TimeZoneName);
            lvi.Tag = L;

            return lvi;
        }

        /// <summary>
        /// new location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNew_Click(object sender, EventArgs e)
        {
            EditLocationPanel d = new EditLocationPanel();
            d.setLocation(null);
            d.PrefferedCountry = m_wndCountry.SelectedItem;
            d.OnEditLocationDone += new TBButtonPressed(OnEditLocationDone);
            EditLocationPanelController dc = new EditLocationPanelController(d);
            dc.ViewContainer = Controller.ViewContainer;

            Controller.ViewContainer.AddControl(dc, GVControlAlign.Center);
        }

        /// <summary>
        /// edit location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listViewLocations.SelectedItems.Count == 0)
                return;

            TLocation loc = listViewLocations.SelectedItems[0].Tag as TLocation;

            EditLocationPanel d = new EditLocationPanel();
            d.setLocation(loc);
            d.PrefferedCountry = m_wndCountry.SelectedItem;
            d.OnEditLocationDone += new TBButtonPressed(OnEditLocationDone);
            EditLocationPanelController dc = new EditLocationPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void OnEditLocationDone(object sender, EventArgs e)
        {
            if (sender is EditLocationPanel)
            {
                TLocationDatabase.Modified = true;
                UpdateLocationList();
            }
            else if (sender is EditLocationPanel.LocationWrapper)
            {
                EditLocationPanel.LocationWrapper lw = (EditLocationPanel.LocationWrapper)sender;
                if (lw.created)
                {
                    TLocationDatabase.LocationList.Add(lw.location);
                    TLocationDatabase.Modified = true;
                }
            }
        }

        /// <summary>
        /// delete location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listViewLocations.SelectedItems.Count == 0)
                return;

            TLocation loc = listViewLocations.SelectedItems[0].Tag as TLocation;

            string ask = string.Format("Do you want to remove location for city \"{0}\" ?", loc.CityName);

            if (MessageBox.Show(ask, "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                TLocationDatabase.LocationList.Remove(loc);
                TLocationDatabase.Modified = true;
                UpdateLocationList();
            }

        }

        /// <summary>
        /// Import
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Import List of Locations:\r\n" +
                "1. First step is to find the file with the list of locations\r\n" +
                "2. Second step is to choose from importing methods (Add or Replace)\r\n" +
                "\r\nDo you want to continue with importing?", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dr != DialogResult.Yes)
                return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "GLOC files (*.gloc)|*.gloc||";

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Importing was cancelled.");
                return;
            }

            dr = MessageBox.Show("Do you want to ADD selected location file to the current database of locations?\r\n" +
                "Press YES for Adding, press NO for replacement of current database with imported file.\r\n" +
                "Press CANCEL for not importing at all.", "Importing method", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dr == DialogResult.Cancel)
                return;

            if (TLocationDatabase.ImportFile(ofd.FileName, (dr == DialogResult.No)) == false)
	        {
		        MessageBox.Show("Importing of file was not succesful.", "Importing progress");
		        return;
	        }

	        // opatovna inicializacia dialog boxu
	        listViewLocations.Items.Clear(); // m_wndList.ResetContent();
	        m_wndCountry.Items.Clear();

	        // setting the current country
	        InitCountries();
	        m_wndCountry.SelectedIndex = 0;
        }

        /// <summary>
        /// Export 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.DefaultExt = ".xml";
            sfd.Filter = "XML file (*.xml)|*.xml||";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                TLocationDatabase.SaveFile( sfd.FileName);
            }
        }

        /// <summary>
        /// Show Google Maps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (listViewLocations.SelectedItems.Count == 0)
                return;

	        TLocation loc = listViewLocations.SelectedItems[0].Tag as TLocation;

		    if (loc != null)
		    {
			    string str = string.Format("<html><head><meta http-equiv=\"REFRESH\" content=\"0;url=http://maps.google.com/?ie=UTF8&ll={0},{1}&spn=0.774196,1.235962&z=10" +
						        "\"></head><body></body><html>", loc.Latitude, loc.Longitude);
			    string fileName = GCGlobal.TemporaryFolderPath;
			    fileName += "temp.html";
                File.WriteAllText(fileName, str);
                System.Diagnostics.Process.Start(fileName);
		    }
        }

        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
	        if (MessageBox.Show("Are you sure to revert list of locations to the internal build-in list of locations?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
	        {
		        TLocationDatabase.SetDefaultDatabase();
                UpdateLocationList();
	        }
        }

        private void onTextFilterChanged(object sender, EventArgs e)
        {
            UpdateLocationList();
        }
    }

    public class LocationsPanelController : GVCore
    {
        public LocationsPanelController(LocationsPanel v)
        {
            View = v;
            v.Controller = this;
        }

        public override Base.Scripting.GSCore ExecuteMessage(string token, Base.Scripting.GSCoreCollection args)
        {
            return base.ExecuteMessage(token, args);
        }
    }

}
