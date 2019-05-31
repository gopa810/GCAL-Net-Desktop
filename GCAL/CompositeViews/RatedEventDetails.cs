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
    public partial class RatedEventDetails : UserControl
    {
        public RatedEventDetailsController Controller {get;set;}
        public GCConfigRatedEvents SelectedConfiguration {get;set;}
        public event TBButtonPressed OnConfigurationChanged;

        public RatedEventDetails()
        {
            InitializeComponent();

            SelectedConfiguration = null;
        }

        public void setConfig(GCConfigRatedEvents ev)
        {
            SelectedConfiguration = ev;
            checkBoxUsePeriod.Checked = ev.useMinPeriodLength;
            checkBoxUseAcceptLimit.Checked = ev.useAcceptLimit;
            numericPeriodLimit.Value = Convert.ToDecimal(ev.minPeriodLength);
            numAcceptLimit.Value = Convert.ToDecimal(ev.acceptLimit);
            textBox1.Text = ev.Title;
            richTextBox1.Text = ev.Description;
            InitComboBoxType();
            comboBoxType.SelectedIndex = 0;
        }

        public void saveConfig()
        {
            GCConfigRatedEvents ev = SelectedConfiguration;
            if (ev == null) return;
            bool mod = false;

            if (ev.useAcceptLimit != checkBoxUseAcceptLimit.Checked)
            {
                ev.useAcceptLimit = checkBoxUseAcceptLimit.Checked;
                mod = true;
            }
            if (ev.useMinPeriodLength != checkBoxUsePeriod.Checked)
            {
                ev.useMinPeriodLength = checkBoxUsePeriod.Checked;
                mod = true;
            }

            if (ev.acceptLimit != Convert.ToDouble(numAcceptLimit.Value))
            {
                ev.acceptLimit = Convert.ToDouble(numAcceptLimit.Value);
                mod = true;
            }
            if (ev.minPeriodLength != Convert.ToInt32(numericPeriodLimit.Value))
            {
                ev.minPeriodLength = Convert.ToInt32(numericPeriodLimit.Value);
                mod = true;
            }
            if (ev.Title == null || !ev.Title.Equals(textBox1.Text))
            {
                ev.Title = textBox1.Text;
                mod = true;
            }
            if (ev.Description == null || !ev.Description.Equals(richTextBox1.Text))
            {
                ev.Description = richTextBox1.Text;
                mod = true;
            }

            if (!mod)
            {
                // we check all entries only in case when main properties were not changed
                // because we just want to know, if this object should be saved to disk
                List<GCConfigRatedEvents.ParameterDescriptor> pd = ev.ParameterDescriptions;
                foreach (GCConfigRatedEvents.ParameterDescriptor p in pd)
                {
                    if (p.Array != null)
                    {
                        for (int i = p.Min; i < p.Max; i++)
                        {
                            if (p.Array[i].Modified)
                            {
                                mod = true;
                                break;
                            }
                        }
                    }
                    else if (p.Array2 != null)
                    {
                        for (int i = p.Min; i < p.Max; i++)
                        {
                            for (int j = p.Min2; j < p.Max2; j++)
                            {
                                if (p.Array[i].Modified)
                                {
                                    mod = true;
                                    break;
                                }
                            }
                            if (mod) break;
                        }
                    }
                    if (mod) break;
                }

            }

            if (mod)
            {
                ev.Save(ev.FileName);
            }
        }

        private void InitComboBoxType()
        {
            GCConfigRatedEvents ev = SelectedConfiguration;
            if (ev == null) return;

            List<GCConfigRatedEvents.ParameterDescriptor> pars = ev.ParameterDescriptions;

            comboBoxType.BeginUpdate();
            comboBoxType.Items.Clear();
            foreach (GCConfigRatedEvents.ParameterDescriptor p in pars)
            {
                comboBoxType.Items.Add(p.Key);
            }
            comboBoxType.EndUpdate();
        }

        private void InitListBoxEntries(GCConfigRatedEvents.ParameterDescriptor p, int index)
        {
            GCConfigRatedEvents ev = SelectedConfiguration;
            if (ev == null) return;

            List<GCConfigRatedEvents.ParameterDescriptor> pars = ev.ParameterDescriptions;
            ListBoxRichItem lbi;

            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            if (p.Array != null)
            {
                for (int i = p.Min; i < p.Max; i++)
                {
                    lbi = new ListBoxRichItem();
                    lbi.Title = p.Array[i].Title;
                    lbi.Subtitle = string.Format("{0}, {1} subratings", p.Key, p.Array[i].MarginsCount);
                    lbi.Tag = p.Array[i];
                    lbi.TitleLevel = 0;
                    listBox1.Items.Add(lbi);
                }
            }
            else if (p.Array2 != null)
            {
                int i = index;
                for (int j = p.Min2; j < p.Max2; j++)
                {
                    lbi = new ListBoxRichItem();
                    lbi.Title = p.Array2[i,j].Title;
                    lbi.Subtitle = string.Format("{0}, {1} subratings", p.Key, p.Array2[i,j].MarginsCount);
                    lbi.Tag = p.Array2[i,j];
                    lbi.TitleLevel = 0;
                    listBox1.Items.Add(lbi);
                }
            }
            listBox1.EndUpdate();
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                e.ItemHeight = (listBox1.Items[e.Index] as ListBoxRichItem).MeasureItem(e.Graphics, listBox1.Bounds, 1);
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                ListBoxRichItem lbi = listBox1.Items[e.Index] as ListBoxRichItem;
                lbi.DrawItem(e, 1);

                if (lbi.Tag != null && lbi.Tag is GCConfigRatedEntry)
                {
                    GCConfigRatedEntry ge = lbi.Tag as GCConfigRatedEntry;
                    string s = ge.Rating.ToString();
                    SizeF ss = e.Graphics.MeasureString(s, ListBoxRichItem.H1Font);
                    Brush b = Brushes.Gray;
                    if (ge.Rating > 0.0) b = Brushes.DarkGreen;
                    else if (ge.Rating < 0.0) b = Brushes.DarkRed;
                    e.Graphics.DrawString(s, ListBoxRichItem.H1Font, b, e.Bounds.Right - 8 - ss.Width, e.Bounds.Top + 8);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveConfig();
            Controller.RemoveFromContainer();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

        private void checkBoxUseAcceptLimit_CheckedChanged(object sender, EventArgs e)
        {
            numAcceptLimit.Visible = checkBoxUseAcceptLimit.Checked;
        }

        private void checkBoxUsePeriod_CheckedChanged(object sender, EventArgs e)
        {
            numericPeriodLimit.Visible = checkBoxUsePeriod.Checked;
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            ListBoxRichItem lbi = listBox1.Items[listBox1.SelectedIndex] as ListBoxRichItem;
            if (lbi.Tag != null && lbi.Tag is GCConfigRatedEntry)
            {
                GCConfigRatedEntry ge = lbi.Tag as GCConfigRatedEntry;
                RatedEventDetailsPanel d = new RatedEventDetailsPanel();
                d.OnValueSaved += new TBButtonPressed(OnRatedEntryEdited);
                d.RatedEvent = ge;
                RatedEventDetailsPanelController dc = new RatedEventDetailsPanelController(d);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        private void OnRatedEntryEdited(object sender, EventArgs e)
        {
            listBox1.Invalidate();
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GCConfigRatedEvents ev = SelectedConfiguration;
            if (ev == null) return;

            List<GCConfigRatedEvents.ParameterDescriptor> pars = ev.ParameterDescriptions;

            if (comboBoxType.SelectedIndex >= 0 && comboBoxType.SelectedIndex < pars.Count)
            {
                GCConfigRatedEvents.ParameterDescriptor p = pars[comboBoxType.SelectedIndex];
                if (p.Array != null)
                {
                    comboBoxSubtype.Visible = false;
                    InitListBoxEntries(p, -1);
                }
                else if (p.Array2 != null)
                {
                    comboBoxSubtype.BeginUpdate();
                    comboBoxSubtype.Items.Clear();
                    comboBoxSubtype.Visible = true;
                    for (int i = p.Min; i < p.Max; i++)
                    {
                        comboBoxSubtype.Items.Add(p.param1Func(i));
                    }
                    comboBoxSubtype.EndUpdate();
                    comboBoxSubtype.SelectedIndex = 0;
                }
            }

        }

        private void comboBoxSubtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            GCConfigRatedEvents ev = SelectedConfiguration;
            if (ev == null) return;

            List<GCConfigRatedEvents.ParameterDescriptor> pars = ev.ParameterDescriptions;

            if (comboBoxType.SelectedIndex >= 0 && comboBoxType.SelectedIndex < pars.Count)
            {
                GCConfigRatedEvents.ParameterDescriptor p = pars[comboBoxType.SelectedIndex];
                InitListBoxEntries(p, comboBoxSubtype.SelectedIndex);
            }
        }

    }

    public class RatedEventDetailsController : GVCore
    {
        public RatedEventDetailsController(RatedEventDetails v)
        {
            v.Controller = this;
            View = v;
        }
    }

    public class ListBoxRichItem
    {
        private static Font p_h1font = null;
        private static Font p_h2font = null;
        private static Font p_textfont = null;
        private static Font p_boldfont = null;

        public static Font H1Font
        {
            get
            {
                if (p_h1font == null)
                    p_h1font = new Font(FontFamily.GenericSansSerif, 14);
                return p_h1font;
            }
        }

        public static Font H2Font
        {
            get
            {
                if (p_h2font == null)
                    p_h2font = new Font(FontFamily.GenericSansSerif, 12);
                return p_h2font;
            }
        }

        public static Font TextFont
        {
            get
            {
                if (p_textfont == null)
                    p_textfont = new Font(FontFamily.GenericSansSerif, 10);
                return p_textfont;
            }
        }

        public static Font BoldFont
        {
            get
            {
                if (p_boldfont == null)
                    p_boldfont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
                return p_boldfont;
            }
        }

        /// <summary>
        /// 0 - text, 1 -> h2, 2 -> h1
        /// </summary>
        public int TitleLevel { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public object Tag { get; set; }

        public static int Padding = 8;
        public static int Spacing = 4;

        public int TitleHeight = 0;

        public ListBoxRichItem()
        {
            TitleLevel = 0;
            Title = string.Empty;
            Subtitle = string.Empty;
            Tag = null;
        }

        public int MeasureItem(Graphics g, Rectangle bounds, int type)
        {
            if (type == 0)
            {
                int startX = 40 - TitleLevel * 20;
                SizeF a1 = g.MeasureString(Title, GetTitleFont(type));
                TitleHeight = (int)a1.Height;
                SizeF a2 = g.MeasureString(Subtitle, TextFont, bounds.Width - startX - Padding);

                return Convert.ToInt32(a1.Height + a2.Height + 2 * Padding + Spacing);
            }
            else if (type == 1)
            {
                int startX = 40 - TitleLevel * 20;
                SizeF a1 = g.MeasureString(Title, GetTitleFont(type));
                TitleHeight = (int)a1.Height;
                SizeF a2 = g.MeasureString(Subtitle, TextFont, bounds.Width - startX - Padding);

                return Convert.ToInt32(a1.Height + a2.Height + 2 * Padding + Spacing);
            }
            else
            {
                return 13;
            }
        }

        public void DrawItem(DrawItemEventArgs e, int type)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = e.Bounds;
            DrawItemState state = e.State;

            if ((state & DrawItemState.Selected) != 0)
            {
                g.FillRectangle(Brushes.LightGreen, bounds);
            }
            else
            {
                g.FillRectangle(SystemBrushes.Window, bounds);
            }

            if (type == 0)
            {
                int startX = 40 + Padding - TitleLevel * 20;

                g.DrawString(Title, GetTitleFont(type), Brushes.Black, startX, bounds.Top + Padding);
                g.DrawString(Subtitle, TextFont, Brushes.Black,
                    new Rectangle(startX, bounds.Top + Padding + TitleHeight + Spacing, bounds.Width - startX - 2*Padding, bounds.Height));
            }
            else if (type == 1)
            {
                int startX = Padding;
                g.DrawString(Title, GetTitleFont(type), Brushes.Black, startX, bounds.Top + Padding);
                g.DrawString(Subtitle, TextFont, Brushes.Black,
                    new Rectangle(startX, bounds.Top + Padding + TitleHeight + Spacing, bounds.Width - startX - 2*Padding, bounds.Height));
            }
        }

        public Font GetTitleFont(int type)
        {
            if (type == 0)
            {
                if (TitleLevel == 2)
                    return H1Font;
                if (TitleLevel == 1)
                    return H2Font;
                return BoldFont;
            }
            else if (type == 1)
            {
                return BoldFont;
            }
            else
            {
                return BoldFont;
            }
        }
    }
}
