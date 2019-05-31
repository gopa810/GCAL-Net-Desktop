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
    public partial class DispSetCalendar : UserControl
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

        public DispSetCalendar()
        {
            int i;

            InitializeComponent();


            displayPairs = new CheckBoxValuePair[] {
                new CheckBoxValuePair(checkBox1, 50),
                new CheckBoxValuePair(checkBox2, 35),
                new CheckBoxValuePair(checkBox3, 20),
                new CheckBoxValuePair(checkBox4, 21),
                new CheckBoxValuePair(checkBox5, 8),
                new CheckBoxValuePair(checkBox6, 7),
                new CheckBoxValuePair(checkBox7, 11),
                new CheckBoxValuePair(checkBox8, 0),
                new CheckBoxValuePair(checkBox9, 1),
                new CheckBoxValuePair(checkBox10, 12),
                new CheckBoxValuePair(checkBox11, 42),
                new CheckBoxValuePair(checkBox12, 17),
                new CheckBoxValuePair(checkBox13, 16),
                new CheckBoxValuePair(checkBox14, 39),
                new CheckBoxValuePair(checkBox15, 36),
                new CheckBoxValuePair(checkBox16, 37),
                new CheckBoxValuePair(checkBox17, 38),
                new CheckBoxValuePair(checkBox18, 41),
                new CheckBoxValuePair(checkBox27, 2),
                new CheckBoxValuePair(checkBox28, 34),
                new CheckBoxValuePair(checkBox29, 3),
                new CheckBoxValuePair(checkBox30, 9),
                new CheckBoxValuePair(checkBox31, 4),
                new CheckBoxValuePair(checkBox32, 5),
                new CheckBoxValuePair(checkBox33, 10),
            };


            comboBox4.SelectedIndex = GCDisplaySettings.Current.CalHeaderType;

            checkBox1.Checked = GCDisplaySettings.Current.EkadasiParanadetails;
            checkBox2.Checked = GCDisplaySettings.Current.NoticeaboutDST;
            checkBox3.Checked = GCDisplaySettings.Current.Donotshowemptydays;
            checkBox4.Checked = GCDisplaySettings.Current.Showbeginingofmasa;
            checkBox5.Checked = GCDisplaySettings.Current.Infoaboutvriddhitithi;
            checkBox6.Checked = GCDisplaySettings.Current.Infoaboutksayatithi;
            checkBox7.Checked = GCDisplaySettings.Current.Ayanamshavalue;
            checkBox8.Checked = GCDisplaySettings.Current.Tithiatarunodaya;
            checkBox9.Checked = GCDisplaySettings.Current.ArunodayaTime;
            checkBox10.Checked = GCDisplaySettings.Current.JulianDay;
            checkBox11.Checked = GCDisplaySettings.Current.OldStyleFastingtext;
            checkBox12.Checked = GCDisplaySettings.Current.EkadasiInfo;
            checkBox13.Checked = GCDisplaySettings.Current.SankrantiInfo;
            checkBox14.Checked = GCDisplaySettings.Current.CalColPaksa;
            checkBox15.Checked = GCDisplaySettings.Current.CalColNaksatra;
            checkBox16.Checked = GCDisplaySettings.Current.CalColYoga;
            checkBox17.Checked = GCDisplaySettings.Current.CalColFast;
            checkBox18.Checked = GCDisplaySettings.Current.CalColMoonRasi;
            checkBox27.Checked = GCDisplaySettings.Current.SunriseTime;
            checkBox28.Checked = GCDisplaySettings.Current.NoonTime;
            checkBox29.Checked = GCDisplaySettings.Current.SunsetTime;
            checkBox30.Checked = GCDisplaySettings.Current.SunLongitude;
            checkBox31.Checked = GCDisplaySettings.Current.MoonriseTime;
            checkBox32.Checked = GCDisplaySettings.Current.MoonsetTime;
            checkBox33.Checked = GCDisplaySettings.Current.MoonLongitude;
            checkBox19.Checked = GCDisplaySettings.Current.CalColCoreEvents;
            checkBox20.Checked = GCDisplaySettings.Current.CalColSunrise;
            checkBox21.Checked = GCDisplaySettings.Current.CalColNoon;
            checkBox22.Checked = GCDisplaySettings.Current.CalColSunset;
        }

        public void Save()
        {
            int i;


            GCDisplaySettings.Current.CalHeaderType = comboBox4.SelectedIndex;

            GCDisplaySettings.Current.EkadasiParanadetails = checkBox1.Checked;
            GCDisplaySettings.Current.NoticeaboutDST = checkBox2.Checked;
            GCDisplaySettings.Current.Donotshowemptydays = checkBox3.Checked;
            GCDisplaySettings.Current.Showbeginingofmasa = checkBox4.Checked;
            GCDisplaySettings.Current.Infoaboutvriddhitithi = checkBox5.Checked;
            GCDisplaySettings.Current.Infoaboutksayatithi = checkBox6.Checked;
            GCDisplaySettings.Current.Ayanamshavalue = checkBox7.Checked;
            GCDisplaySettings.Current.Tithiatarunodaya = checkBox8.Checked;
            GCDisplaySettings.Current.ArunodayaTime = checkBox9.Checked;
            GCDisplaySettings.Current.JulianDay = checkBox10.Checked;
            GCDisplaySettings.Current.OldStyleFastingtext = checkBox11.Checked;
            GCDisplaySettings.Current.EkadasiInfo = checkBox12.Checked;
            GCDisplaySettings.Current.SankrantiInfo = checkBox13.Checked;
            GCDisplaySettings.Current.CalColPaksa = checkBox14.Checked;
            GCDisplaySettings.Current.CalColNaksatra = checkBox15.Checked;
            GCDisplaySettings.Current.CalColYoga = checkBox16.Checked;
            GCDisplaySettings.Current.CalColFast = checkBox17.Checked;
            GCDisplaySettings.Current.CalColMoonRasi = checkBox18.Checked;
            GCDisplaySettings.Current.SunriseTime = checkBox27.Checked;
            GCDisplaySettings.Current.NoonTime = checkBox28.Checked;
            GCDisplaySettings.Current.SunsetTime = checkBox29.Checked;
            GCDisplaySettings.Current.SunLongitude = checkBox30.Checked;
            GCDisplaySettings.Current.MoonriseTime = checkBox31.Checked;
            GCDisplaySettings.Current.MoonsetTime = checkBox32.Checked;
            GCDisplaySettings.Current.MoonLongitude = checkBox33.Checked;
            GCDisplaySettings.Current.CalColCoreEvents = checkBox19.Checked;
            GCDisplaySettings.Current.CalColSunrise = checkBox20.Checked;
            GCDisplaySettings.Current.CalColNoon = checkBox21.Checked;
            GCDisplaySettings.Current.CalColSunset = checkBox22.Checked;

        }
    }

    /// <summary>
    /// Controller for DispSetCalendar
    /// </summary>
    public class DispSetCalendarDelegate : GVCore
    {
        public DispSetCalendarDelegate(DispSetCalendar v)
        {
            View = v;
        }

        public override Base.Scripting.GSCore ExecuteMessage(string token, Base.Scripting.GSCoreCollection args)
        {
            if (token.Equals(GVControlContainer.MsgViewWillHide))
            {
                (View as DispSetCalendar).Save();
                return GVCore.Void;
            }
            else
                return base.ExecuteMessage(token, args);
        }
    }

}
