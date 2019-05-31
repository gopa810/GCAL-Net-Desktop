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
    public partial class DispSetAppDay : UserControl
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

        public DispSetAppDay()
        {
            InitializeComponent();

            displayPairs = new CheckBoxValuePair[] {
                new CheckBoxValuePair(checkBox34, GCDS.APP_CHILDNAMES),
            };

            foreach (CheckBoxValuePair cvp in displayPairs)
            {
                cvp.checkBox.Checked = (GCDisplaySettings.Current.getValue(cvp.dispValue) != 0);
            }
        }

        public void Save()
        {
            foreach (CheckBoxValuePair cvp in displayPairs)
            {
                GCDisplaySettings.Current.setBoolValue(cvp.dispValue, cvp.checkBox.Checked);
            }

        }

    }

    public class DispSetAppDayDelegate : GVCore
    {
        public DispSetAppDayDelegate(DispSetAppDay v)
        {
            View = v;
        }

        public override Base.Scripting.GSCore ExecuteMessage(string token, Base.Scripting.GSCoreCollection args)
        {
            if (token.Equals(GVControlContainer.MsgViewWillHide))
            {
                (View as DispSetAppDay).Save();
                return GVCore.Void;
            }
            else
                return base.ExecuteMessage(token, args);
        }
    }
}
