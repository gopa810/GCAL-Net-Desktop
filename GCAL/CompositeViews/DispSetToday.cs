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
    public partial class DispSetToday : UserControl
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

        private int[] eventGroupPairs;

        public DispSetToday()
        {
            int i;

            InitializeComponent();

            displayPairs = new CheckBoxValuePair[] {
                new CheckBoxValuePair(checkBox19, 45),
                new CheckBoxValuePair(checkBox20, 29),
                new CheckBoxValuePair(checkBox21, 30),
                new CheckBoxValuePair(checkBox22, 31),
                new CheckBoxValuePair(checkBox23, 33),
                new CheckBoxValuePair(checkBox24, 32),
                new CheckBoxValuePair(checkBox25, 46),
                new CheckBoxValuePair(checkBox26, 47),
            };

            foreach (CheckBoxValuePair cvp in displayPairs)
            {
                cvp.checkBox.Checked = (GCDisplaySettings.Current.getValue(cvp.dispValue) != 0);
            }
        }

        public void Save()
        {
            int i;

            foreach (CheckBoxValuePair cvp in displayPairs)
            {
                GCDisplaySettings.Current.setBoolValue(cvp.dispValue, cvp.checkBox.Checked);
            }

        }

    }

    public class DispSetTodayDelegate : GVCore
    {
        public DispSetTodayDelegate(DispSetToday v)
        {
            View = v;
        }

        public override Base.Scripting.GSCore ExecuteMessage(string token, Base.Scripting.GSCoreCollection args)
        {
            if (token.Equals(GVControlContainer.MsgViewWillHide))
            {
                (View as DispSetToday).Save();
                return GVCore.Void;
            }
            else
                return base.ExecuteMessage(token, args);
        }
    }

}
