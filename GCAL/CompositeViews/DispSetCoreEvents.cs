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
    public partial class DispSetCoreEvents : UserControl
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

        public DispSetCoreEvents()
        {
            int i;

            InitializeComponent();

            displayPairs = new CheckBoxValuePair[] {
                new CheckBoxValuePair(checkBox35, GCDS.COREEVENTS_SUN),
                new CheckBoxValuePair(checkBox36, GCDS.COREEVENTS_TITHI),
                new CheckBoxValuePair(checkBox37, GCDS.COREEVENTS_NAKSATRA),
                new CheckBoxValuePair(checkBox38, GCDS.COREEVENTS_YOGA),
                new CheckBoxValuePair(checkBox39, GCDS.COREEVENTS_SANKRANTI),
                new CheckBoxValuePair(checkBox40, GCDS.COREEVENTS_CONJUNCTION),
                new CheckBoxValuePair(checkBox41, GCDS.COREEVENTS_MOON),
                new CheckBoxValuePair(checkBox42, GCDS.COREEVENTS_MOONRASI),
                new CheckBoxValuePair(checkBox43, GCDS.COREEVENTS_RAHUKALAM),
                new CheckBoxValuePair(checkBox44, GCDS.COREEVENTS_YAMAGHANTI),
                new CheckBoxValuePair(checkBox45, GCDS.COREEVENTS_GULIKALAM),
                new CheckBoxValuePair(checkBox46, GCDS.COREEVENTS_ASCENDENT),
                new CheckBoxValuePair(checkBox47, GCDS.COREEVENTS_ABHIJIT_MUHURTA),
            };

            comboBox6.SelectedIndex = GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_SORT);

            foreach (CheckBoxValuePair cvp in displayPairs)
            {
                cvp.checkBox.Checked = (GCDisplaySettings.Current.getValue(cvp.dispValue) != 0);
            }
        }

        public void Save()
        {
            int i;

            GCDisplaySettings.Current.setValue(GCDS.COREEVENTS_SORT, comboBox6.SelectedIndex);

            foreach (CheckBoxValuePair cvp in displayPairs)
            {
                GCDisplaySettings.Current.setBoolValue(cvp.dispValue, cvp.checkBox.Checked);
            }

        }

    }

    public class DispSetCoreEventsDelegate : GVCore
    {
        public DispSetCoreEventsDelegate(DispSetCoreEvents v)
        {
            View = v;
        }

        public override Base.Scripting.GSCore ExecuteMessage(string token, Base.Scripting.GSCoreCollection args)
        {
            if (token.Equals(GVControlContainer.MsgViewWillHide))
            {
                (View as DispSetCoreEvents).Save();
                return GVCore.Void;
            }
            else
                return base.ExecuteMessage(token, args);
        }
    }

}
