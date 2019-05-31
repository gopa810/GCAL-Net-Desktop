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
    public partial class EditLocationPanel : UserControl
    {
        public TLocation SelectedLocation;
        public EditLocationPanelController Controller { get; set; }
        public event TBButtonPressed OnEditLocationDone;

        public EditLocationPanel()
        {
            InitializeComponent();
        }

        public void setLocation(TLocation selloc)
        {
            SelectedLocation = selloc;

            TCountry findCountry = (PrefferedCountry is TCountry ? PrefferedCountry as TCountry : null);
            comboBox1.BeginUpdate();
            foreach (TCountry ct in TCountry.Countries)
            {
                comboBox1.Items.Add(ct);
            }
            comboBox1.Sorted = true;
            comboBox1.EndUpdate();

            comboBox2.BeginUpdate();
            foreach (TTimeZone tz in TTimeZone.TimeZoneList)
            {
                comboBox2.Items.Add(tz);
            }
            comboBox2.EndUpdate();


            if (SelectedLocation == null)
            {
                textBox1.Text = "<new location>";
                textBox2.Text = "12N50";
                textBox3.Text = "50W13";
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }
            else
            {
                textBox1.Text = SelectedLocation.CityName;
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    TCountry tc = comboBox1.Items[i] as TCountry;
                    if (tc.Name.Equals(SelectedLocation.Country.Name))
                    {
                        comboBox1.SelectedIndex = i;
                        break;
                    }
                }
                textBox2.Text = GCEarthData.GetTextLatitude(SelectedLocation.Latitude);
                textBox3.Text = GCEarthData.GetTextLongitude(SelectedLocation.Longitude);
                for (int j = 0; j < comboBox2.Items.Count; j++)
                {
                    TTimeZone tz = comboBox2.Items[j] as TTimeZone;
                    if (tz.Name.Equals(SelectedLocation.TimeZoneName))
                    {
                        comboBox2.SelectedIndex = j;
                        break;
                    }
                }
            }

        }

        private object _prefc;
        public object PrefferedCountry
        {
            get
            {
                return _prefc;
            }
            set
            {
                _prefc = value;
                if (value is TCountry)
                {
                    TCountry tc = value as TCountry;
                    int j = comboBox1.Items.IndexOf(value);
                    if (j >= 0)
                        comboBox1.SelectedIndex = j;
                    else
                        comboBox1.SelectedIndex = 0;

                    // selection of timezone
                    foreach (TLocation loc in TLocationDatabase.LocationList)
                    {
                        if (loc.CountryISOCode.Equals(tc.ISOCode))
                        {
                            for (int k = 0; k < comboBox2.Items.Count; k++)
                            {
                                if (loc.TimeZoneName.Equals((comboBox2.Items[k] as TTimeZone).Name))
                                {
                                    comboBox2.SelectedIndex = k;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.BackColor = IsLatitudeOK() ? Color.LightGreen : Color.LightCoral;
            button1.Enabled = IsLatitudeOK() && IsLongitudeOK();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.BackColor = IsLongitudeOK() ? Color.LightGreen : Color.LightCoral;
            button1.Enabled = IsLatitudeOK() && IsLongitudeOK();
        }

        private bool IsLatitudeOK()
        {
            double d;
            return ToDouble(textBox2.Text, out d, 'N', 'S');
        }

        private bool IsLongitudeOK()
        {
            double d;
            return ToDouble(textBox3.Text, out d, 'E', 'W');
        }

        private bool ToDouble(string s, out double val, char poschar, char negchar)
        {
            int i;

            i = s.IndexOf(negchar);
            if (i >= 0)
                return ToDouble2(s, i, -1.0, out val);
            i = s.IndexOf(poschar);
            if (i >= 0)
                return ToDouble2(s, i, 1.0, out val);

            val = 0.0;
            return false;
        }

        private bool ToDouble2(string s, int i, double sig, out double val)
        {
            int a, b;
            if (int.TryParse(s.Substring(0, i), out a))
            {
                if (int.TryParse(s.Substring(i + 1), out b))
                {
                    val = sig * (a * 60.0 + b * 1.0) / 60.0;
                    return true;
                }
            }

            val = 0.0;
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LocationWrapper lw = new LocationWrapper(SelectedLocation);
            if (SelectedLocation == null)
                SelectedLocation = lw.location;

            double d;
            SelectedLocation.CityName = textBox1.Text;
            SelectedLocation.CountryISOCode = (comboBox1.SelectedItem as TCountry).ISOCode;
            ToDouble(textBox2.Text, out d, 'N', 'S');
            SelectedLocation.Latitude = d;
            ToDouble(textBox3.Text, out d, 'E', 'W');
            SelectedLocation.Longitude = d;
            TTimeZone tz = comboBox2.SelectedItem as TTimeZone;
            SelectedLocation.TimeZone = tz;

            if (OnEditLocationDone != null)
                OnEditLocationDone(lw, e);

            Controller.RemoveFromContainer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

        public class LocationWrapper
        {
            public bool created = false;
            public TLocation location = null;
            public LocationWrapper(TLocation loc)
            {
                created = (loc == null);
                location = (loc == null ? new TLocation() : loc);
            }
        }

    }

    public class EditLocationPanelController : GVCore
    {
        public EditLocationPanelController(EditLocationPanel v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
