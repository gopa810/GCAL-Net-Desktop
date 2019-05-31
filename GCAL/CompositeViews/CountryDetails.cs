using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;
using GCAL.Views;
using GCAL.Base.Scripting;

namespace GCAL.CompositeViews
{
    public partial class CountryDetails : UserControl
    {
        public CountryDetailsController Controller { get; set; }
        public event TBButtonPressed OnButtonSave;

        public CountryDetails()
        {
            InitializeComponent();

            comboBox1.BeginUpdate();
            foreach (TContinent continent in TContinent.Continents)
            {
                comboBox1.Items.Add(continent);
            }
            comboBox1.EndUpdate();
        }

        private TCountry _sc;

        public TCountry SelectedCountry
        {
            get
            {
                return _sc;
            }
            set
            {
                _sc = value;
                if (_sc != null)
                {
                    textBox1.Enabled = false;
                    textBox1.Text = _sc.ISOCode;
                    textBox2.Text = _sc.Name;
                    SelectedContinent = _sc.Continent;
                    comboBox2.SelectedIndex = _sc.FirstDayOfWeek;
                }
            }
        }

        public TContinent SelectedContinent
        {
            get
            {
                return (TContinent)comboBox1.SelectedItem;
            }
            set
            {
                int i;
                for (i = 0; i < comboBox1.Items.Count; i++)
                {
                    TContinent c = comboBox1.Items[i] as TContinent;
                    if (c.ISOCode.Equals(value.ISOCode))
                    {
                        comboBox1.SelectedIndex = i;
                        i = -1;
                        break;
                    }
                }
                if (i > 0)
                    comboBox1.SelectedIndex = 0;
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool ok = IsCodeCorrect() && IsCodeUnique();

            textBox1.BackColor = ok ? Color.LightGreen : Color.LightCoral;

            buttonSave.Enabled = ok && IsNameCorrect();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool ok = IsNameCorrect();

            textBox2.BackColor = ok ? Color.LightGreen : Color.LightCoral;

            buttonSave.Enabled = IsCodeCorrect() && IsCodeUnique() && ok;

        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (_sc == null)
            {
                TCountry tc = new TCountry();
                tc.ISOCode = textBox1.Text;
                tc.Name = textBox2.Text;
                tc.ContinentISOCode = SelectedContinent.ISOCode;
                TCountry.Countries.Add(tc);
            }
            else
            {
                _sc.ISOCode = textBox1.Text;
                _sc.Name = textBox2.Text;
                _sc.ContinentISOCode = SelectedContinent.ISOCode;
            }
            TCountry.Modified = true;

            Controller.RemoveFromContainer();
            if (OnButtonSave != null)
                OnButtonSave(this, e);
        }


        private bool IsCodeCorrect()
        {
            string s = textBox1.Text;

            if (s.Length != 2)
                return false;

            if (!Char.IsLetter(s[0]) || !Char.IsLetter(s[1]))
                return false;

            return true;
        }

        private bool IsCodeUnique()
        {
            if (textBox1.Enabled == false)
                return true;

            TCountry tc = TCountry.FindCountryByISOCode(textBox1.Text);
            return tc == null;
        }

        private bool IsNameCorrect()
        {
            return textBox2.Text.Trim().Length > 0;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_sc != null)
            {
                _sc.FirstDayOfWeek = comboBox2.SelectedIndex;
            }
        }
    }

    public class CountryDetailsController : GVCore
    {
        public CountryDetailsController(CountryDetails v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
