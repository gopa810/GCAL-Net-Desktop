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
    public partial class EditEventSpecial : UserControl
    {
        public EditEventSpecial()
        {
            InitializeComponent();

            for (int i = 0; i < 12; i++)
            {
                comboBox1.Items.Add(string.Format("{0} Masa", GCMasa.GetName(i)));
                comboBox2.Items.Add(string.Format("{0} Masa", GCMasa.GetName(i)));
            }
        }

        public void setEvent(GCFestivalSpecial ev)
        {
            comboBox1.SelectedIndex = ev.nMasaMin;
            comboBox2.SelectedIndex = ev.nMasaMax;
            richTextBox1.Text = ev.Script;
        }

        public void updateEvent(GCFestivalSpecial ev)
        {
            ev.nMasaMin = comboBox1.SelectedIndex;
            ev.nMasaMax = comboBox2.SelectedIndex;
        }
    }
}
