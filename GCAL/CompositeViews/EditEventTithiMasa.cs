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
    public partial class EditEventTithiMasa : UserControl
    {
        public EditEventTithiMasa()
        {
            InitializeComponent();

            for (int i = 0; i < 30; i++)
            {
                comboBox1.Items.Add(string.Format("{0} Tithi ({1} Paksa)", GCTithi.GetName(i), GCPaksa.GetName(i / 15))); 
            }

            for (int i = 0; i < 12; i++)
            {
                comboBox2.Items.Add(string.Format("{0} Masa", GCMasa.GetName(i)));
            }
        }

        public void setEvent(GCFestivalTithiMasa ev)
        {
            comboBox1.SelectedIndex = ev.nTithi;
            comboBox2.SelectedIndex = ev.nMasa;
        }

        public bool updateEvent(GCFestivalTithiMasa ev)
        {
            bool changed = (ev.nTithi != comboBox1.SelectedIndex) || (ev.nMasa != comboBox2.SelectedIndex);
            ev.nTithi = comboBox1.SelectedIndex;
            ev.nMasa = comboBox2.SelectedIndex;
            return changed;
        }
    }
}
