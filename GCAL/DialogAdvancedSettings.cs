using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;
namespace GCAL
{
    public partial class DialogAdvancedSettings : Form
    {
        public GCCoreAstronomy.AstronomySystem SystemB { get; set; }
        public DialogAdvancedSettings()
        {
            InitializeComponent();

            comboBox1.Items.Clear();

            AddSystemComboValue(GCCoreAstronomy.AstronomySystem.Meeus);
            //AddSystemComboValue(GCCoreAstronomy.AstronomySystem.SuryaSiddhanta);

            checkBox1.Checked = GCDisplaySettings.Current.AdvFestivalFirstDay;

        }

        private void AddSystemComboValue(GCCoreAstronomy.AstronomySystem val)
        {
            comboBox1.Items.Add(val);
            if (GCCoreAstronomy.System == val)
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("CONFIRM"))
            {
                SystemB = (GCCoreAstronomy.AstronomySystem)comboBox1.Items[comboBox1.SelectedIndex];

                GCDisplaySettings.Current.AdvFestivalFirstDay = checkBox1.Checked;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = textBox1.Text.Equals("CONFIRM");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
