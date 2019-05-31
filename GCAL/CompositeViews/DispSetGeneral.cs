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

namespace GCAL.CompositeViews
{
    public partial class DispSetGeneral : UserControl
    {
        private class CheckBoxValuePair
        {
            public CheckBox checkBox;
            public int dispValue;
            public CheckBoxValuePair(CheckBox cb, int dv)
            {
                checkBox = cb;
                dispValue = dv;
            }
        }

        private CheckBoxValuePair[] displayPairs;

        public DispSetGeneral()
        {
            int i;

            InitializeComponent();

            displayPairs = new CheckBoxValuePair[] {
            };

            // first day of week
            for (i = 0; i < 7; i++)
            {
                comboBox1.Items.Add(GCCalendar.GetWeekdayName(i));
            }
            comboBox1.SelectedIndex = GCDisplaySettings.Current.getValue(GCDS.GENERAL_FIRST_DOW);

            // masa name format
            comboBox2.SelectedIndex = GCDisplaySettings.Current.getValue(GCDS.GENERAL_MASA_FORMAT);

            // anniversary format
            comboBox3.SelectedIndex = GCDisplaySettings.Current.getValue(GCDS.GENERAL_ANNIVERSARY_FMT);

            foreach (CheckBoxValuePair cvp in displayPairs)
            {
                cvp.checkBox.Checked = (GCDisplaySettings.Current.getValue(cvp.dispValue) != 0);
            }
        }

        public void Save()
        {
            int i;

            GCDisplaySettings.Current.setValue(GCDS.GENERAL_FIRST_DOW, comboBox1.SelectedIndex);

            // masa name format
            GCDisplaySettings.Current.setValue(GCDS.GENERAL_MASA_FORMAT, comboBox2.SelectedIndex);

            // anniversary format
            GCDisplaySettings.Current.setValue(GCDS.GENERAL_ANNIVERSARY_FMT, comboBox3.SelectedIndex);

            foreach (CheckBoxValuePair cvp in displayPairs)
            {
                GCDisplaySettings.Current.setBoolValue(cvp.dispValue, cvp.checkBox.Checked);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogAdvancedSettings dlg = new DialogAdvancedSettings();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                GCCoreAstronomy.System = dlg.SystemB;
                Properties.Settings.Default.CoreAstroSystem = (int)dlg.SystemB;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to reset all display settings to default values?", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GCDisplaySettings.Current.Reset();
                GCDisplaySettings.SaveDisplaySettingsFile(GCGlobal.DisplaySettingsFilePath);
            }
        }
    }

    public class DispSetGeneralDelegate : GVCore
    {
        public DispSetGeneralDelegate(DispSetGeneral v)
        {
            View = v;
        }

        public override Base.Scripting.GSCore ExecuteMessage(string token, Base.Scripting.GSCoreCollection args)
        {
            if (token.Equals(GVControlContainer.MsgViewWillHide))
            {
                (View as DispSetGeneral).Save();
                return GVCore.Void;
            }
            else
                return base.ExecuteMessage(token, args);
        }
    }

}
