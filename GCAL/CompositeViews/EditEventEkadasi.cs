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
    public partial class EditEventEkadasi : UserControl
    {
        public EditEventEkadasi()
        {
            InitializeComponent();

            for (int i = 0; i < 12; i++)
            {
                comboBox1.Items.Add(string.Format("{0}", GCMasa.GetName(i)));
            }

            for (int i = 0; i < 2; i++)
            {
                comboBox2.Items.Add(string.Format("{0}", GCPaksa.GetName(i)));
            }

        }

        public void setEvent(GCFestivalEkadasi ev)
        {
            comboBox1.SelectedIndex = ev.nMasa;
            comboBox2.SelectedIndex = ev.nPaksa;
        }

        public bool updateEvent(GCFestivalEkadasi ev)
        {
            bool b = (ev.nMasa != comboBox1.SelectedIndex) || (ev.nPaksa != comboBox2.SelectedIndex);
            ev.nMasa = comboBox1.SelectedIndex;
            ev.nPaksa = comboBox2.SelectedIndex;
            return b;
        }
    }
}
