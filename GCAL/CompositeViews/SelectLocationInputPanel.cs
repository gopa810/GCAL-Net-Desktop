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
    public partial class SelectLocationInputPanel : UserControl
    {

        public int PaddingX = 8;
        public int Spacing = 4;
        private Font CaptionFont = null;
        private Font TextFont = null;

        public SelectLocationInputPanelController Controller { get; set; }

        public event TBButtonPressed OnLocationSelected;

        public SelectLocationInputPanel()
        {
            InitializeComponent();
            CaptionFont = new Font(FontFamily.GenericSansSerif, 13);
            TextFont = new Font(FontFamily.GenericSansSerif, 10);

            foreach (GCLocation loc in GCGlobal.recentLocations)
            {
                listBox1.Items.Add(loc);
            }
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {

            SizeF a = e.Graphics.MeasureString("A", CaptionFont);
            SizeF b = e.Graphics.MeasureString("B", TextFont);

            e.ItemHeight = 2 * PaddingX + Spacing + Convert.ToInt32(a.Height + b.Height);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < GCGlobal.recentLocations.Count)
            {
                GCLocation cr = GCGlobal.recentLocations[e.Index];

                if ((e.State & DrawItemState.Selected) != 0)
                {
                    e.Graphics.FillRectangle(Brushes.LightYellow, e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
                }

                SizeF a = e.Graphics.MeasureString("A", CaptionFont);
                e.Graphics.DrawString(cr.Title, CaptionFont, Brushes.Black, PaddingX, e.Bounds.Top + PaddingX);
                e.Graphics.DrawString(cr.Format("{longitudeText} {latitudeText}, {timeZoneName}"),
                    TextFont, Brushes.Gray, PaddingX, e.Bounds.Top + PaddingX + a.Height + Spacing);
            }
        }

        public void HideMyLocation()
        {
            button3.Visible = false;
            label3.Visible = false;
        }

        public bool VisibleMyLocation
        {
            set
            {
                label3.Visible = value;
                button3.Visible = value;
            }
            get
            {
                return button3.Visible;
            }
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            LocationChoosePanel d = new LocationChoosePanel();
            d.OnLocationSelected += this.OnLocationSelected;
            ChooseLocationPanelController dc = new ChooseLocationPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
            Controller.RemoveFromContainer();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            LocationEnterPanel d = new LocationEnterPanel();
            d.OnLocationSelected += this.OnLocationSelected;
            LocationEnterPanelController dc = new LocationEnterPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
            Controller.RemoveFromContainer();
        }

        private void buttonMyLocation_Click(object sender, EventArgs e)
        {
            if (OnLocationSelected != null)
                OnLocationSelected(GCGlobal.myLocation, e);
            Controller.RemoveFromContainer();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Controller != null && listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
            {
                if (OnLocationSelected != null)
                    OnLocationSelected(GCGlobal.recentLocations[listBox1.SelectedIndex], e);
                Controller.RemoveFromContainer();
            }
        }
    }


    public class SelectLocationInputPanelController : GVCore
    {
        public SelectLocationInputPanelController(SelectLocationInputPanel v)
        {
            View = v;
            v.Controller = this;
        }

    }
}
