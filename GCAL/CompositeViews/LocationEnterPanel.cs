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
    public partial class LocationEnterPanel : UserControl
    {
        private GCLocation loc = new GCLocation();
        public LocationEnterPanelController Controller { get; set; }
        public event TBButtonPressed OnLocationSelected;

        private bool b_upd = false;
        private double signOfLatitude = 1.0;
        private double signOfLongitude = 1.0;


        public LocationEnterPanel()
        {
            InitializeComponent();

            if (TTimeZone.TimeZoneList != null)
            {
                foreach (TTimeZone tz in TTimeZone.TimeZoneList)
                {
                    cbTimezones.Items.Add(tz);
                }
            }
        }

        /// <summary>
        /// Gets or sets main edited data in the panel.
        /// </summary>
        public GCLocation LocationRef
        {
            get
            {
                loc = new GCLocation();

                loc.Title = textBox1.Text;
                loc.Latitude = DoubleFromDialog(tbLatDeg, tbLatArc, signOfLatitude);
                loc.Longitude = DoubleFromDialog(tbLongDeg, tbLongArc, signOfLongitude);

                TTimeZone tz = cbTimezones.SelectedItem as TTimeZone;
                if (tz != null)
                {
                    loc.TimeZone = tz;
                }

                return loc;
            }
            set
            {
                if (value == null)
                    loc = new GCLocation();
                else
                    loc = value;
                b_upd = true;
                textBox1.Text = loc.Title;
                DialogLatitude = loc.Latitude;
                DialogLongitude = loc.Longitude;
                SelectTimezone(loc.TimeZone);
                b_upd = false;
            }
        }


        public double DoubleFromDialog(TextBox t1, TextBox t2, double sig)
        {
            double a;
            int b;
            if (double.TryParse(t1.Text, out a) && int.TryParse(t2.Text, out b))
            {
                return sig * (a + b / 60.0);
            }

            return 0.0;
        }

        public double DialogLatitude
        {
            get
            {
                return DoubleFromDialog(tbLatDeg, tbLatArc, signOfLatitude);
            }
            set
            {
                double a = value;
                if (a < 0.0)
                {
                    signOfLatitude = -1.0;
                    a = -a;
                }
                else
                {
                    signOfLatitude = 1.0;
                }
                tbLatDeg.Text = GCMath.IntFloor(a).ToString("00");
                tbLatArc.Text = GCMath.IntFloor((a - Math.Floor(a))*60.0).ToString("00");
                btnLatDir.Text = GCStrings.getLatitudeDirectionName(signOfLatitude);
            }
        }

        public double DialogLongitude
        {
            get
            {
                return DoubleFromDialog(tbLongDeg, tbLongArc, signOfLongitude);
            }
            set
            {
                double a = value;
                if (a < 0.0)
                {
                    signOfLongitude = -1.0;
                    a = -a;
                }
                else
                {
                    signOfLongitude = 1.0;
                }
                tbLongDeg.Text = GCMath.IntFloor(a).ToString("00");
                tbLongArc.Text = GCMath.IntFloor((a - Math.Floor(a)) * 60.0).ToString("00");
                btnLongDir.Text = GCStrings.getLongitudeDirectionName(signOfLongitude);
            }
        }

        private void SelectTimezone(TTimeZone timeZone)
        {
             cbTimezones.SelectedItem = timeZone;
        }

        public GCEarthData GetEarthData()
        {
            GCLocation L = LocationRef;
            return L.GetEarthData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            UpdateDSTInfo();
        }

        private void UpdateDSTInfo()
        {
            TTimeZone tz = cbTimezones.SelectedItem as TTimeZone;
            if (tz != null)
            {
                labelDstInfo.Text = tz.HumanDstText();
            }
        }

        private int UpdateDstByTimezone(double tzone)
        {
            if (!checkBox1.Checked)
                return 0;

            if (loc == null)
                return 0;

            double lon = (Math.Abs(DialogLongitude) + 7.5) / 15.0;
            if (DialogLongitude < 0.0)
                lon = -lon;

            foreach (TTimeZone tz in cbTimezones.Items)
            {
                if (tz.OffsetMinutes >= (int)lon * 60)
                {
                    cbTimezones.SelectedItem = tz;
                    break;
                }
            }

            UpdateDSTInfo();

            return 1;
        }

        // button longitude
        private void buttonLongDir_Click(object sender, EventArgs e)
        {
            signOfLongitude = -signOfLongitude;
            UpdatedLongitude();
            btnLongDir.Text = GCStrings.getLongitudeDirectionName(signOfLongitude);
        }

        private void UpdatedLongitude()
        {
            if (loc == null)
                return;

            if (GCUserInterface.dstSelectionMethod == 2 && b_upd == true)
            {
                loc.Longitude = DoubleFromDialog(tbLongDeg, tbLongArc, signOfLongitude);
                UpdateDstByTimezone(loc.OffsetUtcHours);
            }
        }

        // button latitude
        private void buttonLatDir_Click(object sender, EventArgs e)
        {
            signOfLatitude = -signOfLatitude;
            UpdatedLatitude();
            btnLatDir.Text = GCStrings.getLatitudeDirectionName(signOfLatitude);
        }

        private void UpdatedLatitude()
        {
            if (loc == null)
                return;

            if (GCUserInterface.dstSelectionMethod == 2 && b_upd == true)
            {
                loc.Latitude = DoubleFromDialog(tbLatDeg, tbLatArc, signOfLatitude);
                UpdateDstByTimezone(loc.OffsetUtcHours);
            }
        }

        private bool IsCorrect(TextBox tb, int min, int max)
        {
            int a;
            if (int.TryParse(tb.Text, out a))
            {
                return ((min <= a) && (a <= max));
            }
            else
            {
                return false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (IsCorrect(tbLatDeg, 0, 89))
            {
                tbLatDeg.BackColor = SystemColors.Window;
                UpdatedLatitude();
            }
            else
            {
                tbLatDeg.BackColor = Color.LightCoral;
                labelDstInfo.Text = "Latitude degrees only between 0 and 89.";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (IsCorrect(tbLatArc, 0, 59))
            {
                tbLatArc.BackColor = SystemColors.Window;
                UpdatedLatitude();
            }
            else
            {
                tbLatArc.BackColor = Color.LightCoral;
                labelDstInfo.Text = "Latitude minutes only between 0 and 59";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (IsCorrect(tbLongDeg, 0, 179))
            {
                tbLongDeg.BackColor = SystemColors.Window;
                UpdatedLongitude();
            }
            else
            {
                tbLongDeg.BackColor = Color.LightCoral;
                labelDstInfo.Text = "Longitude degrees only between 0 and 179.";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (IsCorrect(tbLongArc, 0, 59))
            {
                tbLongArc.BackColor = SystemColors.Window;
                UpdatedLongitude();
            }
            else
            {
                tbLongArc.BackColor = Color.LightCoral;
                labelDstInfo.Text = "Latitude minutes only between 0 and 59";
            }
        }

        public bool ButtonOkEnable
        {
            get
            {
                return button1.Visible;
            }
            set
            {
                button1.Visible = value;
            }
        }

        public bool ButtonCancelEnable
        {
            get
            {
                return button2.Visible;
            }
            set
            {
                button2.Visible = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OnLocationSelected != null)
                OnLocationSelected(LocationRef, e);
            Controller.RemoveFromContainer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }
    }

    public class LocationEnterPanelController : GVCore
    {
        public LocationEnterPanelController(LocationEnterPanel v)
        {
            View = v;
            v.Controller = this;
            v.ButtonCancelEnable = true;
            v.ButtonOkEnable = true;
        }

    }

}
