using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base.Scripting;
using GCAL.Views;
using GCAL.Base;

namespace GCAL.CompositeViews
{
    public partial class StartDatePanel : UserControl
    {
        public bool SupressAutoUpdating = true;

        public bool CorrectDates = true;
        public GCEarthData EarthLocation;

        public StartDatePanelController Controller { get; set; }

        public event TBButtonPressed OnStartDateDone;

        public StartDatePanel()
        {
            InitializeComponent();
            int i;

            for (i = 1; i <= 31; i++) cbDay.Items.Add(i.ToString());
            for(i = 1; i <= 12; i++) cbMonth.Items.Add(GregorianDateTime.GetMonthName(i));
            for (i = 0; i < 30; i++) cbTithi.Items.Add(GCTithi.GetName(i));
            for (i = 0; i <= 12; i++) cbMasa.Items.Add(GCMasa.GetName(i));
        }

        public string CustomTitle
        {
            set
            {
                label7.Text = value;
            }

            get
            {
                return label7.Text;
            }
        }

        public GregorianDateTime GregorianTime
        {
            get
            {
                return GetGregorianValues();
            }
            set
            {
                SupressAutoUpdating = true;
                SetGregorianValues(value);
                UpdateGaurabdaValues(value);
                SupressAutoUpdating = false;
            }
        }


        public GaurabdaDate GaurabdaTime
        {
            get
            {
                GaurabdaDate va = GetGaurabdaValues();
                return va;
            }
            set
            {
                SupressAutoUpdating = true;
                SetGaurabdaValues(value);
                UpdateGregorianValues(value);
                SupressAutoUpdating = false;
            }
        }

        private void UpdateGregorianValues(GaurabdaDate value)
        {
            if (EarthLocation != null)
            {
                GregorianDateTime vc;
                GCCalendar.VATIMEtoVCTIME(value, out vc, EarthLocation);
                SetGregorianValues(vc);
            }
        }

        private void UpdateGaurabdaValues(GregorianDateTime value)
        {
            if (EarthLocation != null)
            {
                GaurabdaDate va;
                GCCalendar.VCTIMEtoVATIME(value, out va, EarthLocation);
                SetGaurabdaValues(va);
            }
        }
        
        private GregorianDateTime GetGregorianValues()
        {
            GregorianDateTime vc = new GregorianDateTime();
            vc.day = cbDay.SelectedIndex + 1;
            vc.month = cbMonth.SelectedIndex + 1;
            int.TryParse(tbYear.Text, out vc.year);
            vc.InitWeekDay();
            vc.TimezoneHours = 0.0;
            return vc;
        }

        private GaurabdaDate GetGaurabdaValues()
        {
            GaurabdaDate va = new GaurabdaDate();
            va.tithi = cbTithi.SelectedIndex;
            va.masa = cbMasa.SelectedIndex;
            int.TryParse(tbGaurabdaYear.Text, out va.gyear);
            return va;
        }

        private void SetGregorianValues(GregorianDateTime value)
        {
            cbDay.SelectedIndex = value.day - 1;
            cbMonth.SelectedIndex = value.month - 1;
            tbYear.Text = value.year.ToString();
        }

        private void SetGaurabdaValues(GaurabdaDate value)
        {
            cbTithi.SelectedIndex = value.tithi;
            cbMasa.SelectedIndex = value.masa;
            tbGaurabdaYear.Text = value.gyear.ToString();
        }

        private void linkStartYear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cbDay.SelectedIndex = 0;
            cbMonth.SelectedIndex = 0;
        }

        private void linkStartMonth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cbDay.SelectedIndex = 0;
        }

        private void linkYearPlus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int a;
            if (int.TryParse(tbYear.Text, out a))
            {
                tbYear.Text = (a + 1).ToString();
            }
        }

        private void linkYearMinus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int a;
            if (int.TryParse(tbYear.Text, out a))
            {
                tbYear.Text = (a - 1).ToString();
            }
        }

        private void linkStartMasa_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cbTithi.SelectedIndex = 0;
        }

        private void linkStartGaurabda_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cbTithi.SelectedIndex = 0;
            cbMasa.SelectedIndex = 0;
        }

        private void cbDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SupressAutoUpdating) return;

            SupressAutoUpdating = true;
            UpdateGaurabdaValues(GetGregorianValues());
            SupressAutoUpdating = false;
        }

        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SupressAutoUpdating) return;

            SupressAutoUpdating = true;
            UpdateGaurabdaValues(GetGregorianValues());
            SupressAutoUpdating = false;
        }

        private void tbYear_TextChanged(object sender, EventArgs e)
        {
            if (SupressAutoUpdating) return;

            SupressAutoUpdating = true;
            int a;
            if (int.TryParse(tbYear.Text, out a))
            {
                if (a >= 1500 && a < 4000)
                {
                    EnableGaurabda(true);
                    UpdateGaurabdaValues(GetGregorianValues());
                }
                else
                    EnableGaurabda(false);
            }
            else
            {
                EnableGaurabda(false);
            }
            SupressAutoUpdating = false;
        }

        private void EnableGregorian(bool b)
        {
            cbDay.Visible = b;
            cbMonth.Visible = b;
            tbYear.Visible = b;
            label1.Visible = b;
            label2.Visible = b;
            label3.Visible = b;
            CorrectDates = b;
        }

        private void EnableGaurabda(bool b)
        {
            cbTithi.Visible = b;
            cbMasa.Visible = b;
            tbGaurabdaYear.Visible = b;
            label4.Visible = b;
            label5.Visible = b;
            label6.Visible = b;
            CorrectDates = b;
        }

        private void cbTithi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SupressAutoUpdating) return;

            SupressAutoUpdating = true;
            UpdateGregorianValues(GetGaurabdaValues());
            SupressAutoUpdating = false;
        }

        private void cbMasa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SupressAutoUpdating) return;

            SupressAutoUpdating = true;
            UpdateGregorianValues(GetGaurabdaValues());
            SupressAutoUpdating = false;
        }

        private void tbGaurabdaYear_TextChanged(object sender, EventArgs e)
        {
            if (SupressAutoUpdating) return;

            SupressAutoUpdating = true;
            
            int a;
            if (int.TryParse(tbGaurabdaYear.Text, out a))
            {
                if (a >= 0 && a < 2500)
                {
                    EnableGregorian(true);
                    UpdateGregorianValues(GetGaurabdaValues());
                }
                else
                    EnableGregorian(false);
            }
            else
            {
                EnableGregorian(false);
            }

            SupressAutoUpdating = false;
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
            if (OnStartDateDone != null)
                OnStartDateDone(GregorianTime, e);
            Controller.RemoveFromContainer();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

    }

    public class StartDatePanelController : GVCore
    {
        public StartDatePanelController(StartDatePanel v)
        {
            View = v;
            v.EarthLocation = GCGlobal.LastLocation.GetEarthData();
            v.Controller = this;
            v.ButtonCancelEnable = true;
            v.ButtonOkEnable = true;
        }
    }
}
