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
    public partial class CountriesPanel : UserControl
    {
        public CountriesPanelDelegate Controller { get; set; }
        public GVControlContainer ViewContainer { get; set; }

        public CountriesPanel()
        {
            InitializeComponent();


            InitCountryList();
        }

        public void InitCountryList()
        {
            listView1.Items.Clear();
            listView1.BeginUpdate();
            foreach (TCountry tc in TCountry.Countries)
            {
                ListViewItem lvi = new ListViewItem(tc.ISOCode);
                lvi.SubItems.Add(tc.Name);
                lvi.Tag = tc;
                listView1.Items.Add(lvi);
            }
            listView1.EndUpdate();
        }

        /// <summary>
        /// New
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            CountryDetails d = new CountryDetails();
            d.OnButtonSave += new TBButtonPressed(OnCountryListChanged);
            d.SelectedCountry = null;

            CountryDetailsController dc = new CountryDetailsController(d);

            dc.ShowInContainer(ViewContainer, GVControlAlign.Fill);
        }

        private void OnCountryListChanged(object sender, EventArgs e)
        {
            InitCountryList();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            TCountry sc = listView1.SelectedItems[0].Tag as TCountry;

            CountryDetails d = new CountryDetails();
            d.OnButtonSave += new TBButtonPressed(OnCountryListChanged);
            d.SelectedCountry = sc;

            CountryDetailsController dc = new CountryDetailsController(d);

            dc.ShowInContainer(ViewContainer, GVControlAlign.Fill);
        }

    }

    public class CountriesPanelDelegate : GVCore
    {
        public CountriesPanelDelegate(CountriesPanel v)
        {
            View = v;
        }

        public override Base.Scripting.GSCore ExecuteMessage(string token, Base.Scripting.GSCoreCollection args)
        {
            return base.ExecuteMessage(token, args);
        }
    }
}
