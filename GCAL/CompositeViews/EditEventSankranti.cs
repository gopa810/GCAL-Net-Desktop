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
    public partial class EditEventSankranti : UserControl
    {
        public EditEventSankranti()
        {
            InitializeComponent();

            for (int i = 0; i < 12; i++)
            {
                comboBox1.Items.Add(string.Format("{0} Sankranti", GCRasi.GetName(i)));
            }

        }

        public void setEvent(GCFestivalSankranti ev)
        {
            comboBox1.SelectedIndex = ev.RasiOfSun;
        }

        public bool updateEvent(GCFestivalSankranti ev)
        {
            bool b = (ev.RasiOfSun != comboBox1.SelectedIndex);
            ev.RasiOfSun = comboBox1.SelectedIndex;
            return b;
        }
    }
}
