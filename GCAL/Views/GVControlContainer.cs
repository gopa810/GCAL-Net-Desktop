using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base.Scripting;

namespace GCAL.Views
{
    public interface IControlContainer
    {
        void AddControl(GVCore core, GVControlAlign align);
        void RemoveControl(GVCore core);
        void RemoveAll();
    }

    public enum GVControlAlign
    {
        Fill,
        Center,
        Scroll
    }

    public partial class GVControlContainer : UserControl, IControlContainer
    {
        public const string MsgViewWillAppear = "viewWillAppear";
        public const string MsgViewWillHide = "viewWillHide";

        public class ControlRecord
        {
            public GVCore Controller { get; set; }
            public GVControlAlign Align { get; set; }
            public Size OriginalSize { get; set; }

            public ControlRecord()
            {
                Align = GVControlAlign.Center;
            }
        }

        private List<ControlRecord> gvControls = new List<ControlRecord>();
        public GVCore Controller { get; set; }

        public GVControlContainer()
        {
            InitializeComponent();
        }

        private void GVCenteredChildControl_SizeChanged(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
            {
                ControlRecord child = LastControl;
                LayoutControl(child.Controller, child.Align, child.OriginalSize);
            }
        }

        private ControlRecord LastControl
        {
            get
            {
                return gvControls[gvControls.Count - 1];
            }
        }

        public void AddControl(GVCore core, GVControlAlign align)
        {
            ControlRecord cr = new ControlRecord();
            cr.Controller = core;
            cr.Align = align;
            cr.OriginalSize = core.View.Size;

            core.ExecuteMessage(MsgViewWillAppear);
            HideControlsAll(true);
            gvControls.Add(cr);
            core.View.Parent = panel1;
            core.View.Visible = true;
            panel1.Controls.Add(core.View);
            InitialLayoutControl(core, align, cr.OriginalSize);
        }

        public void RemoveControl(GVCore ctr)
        {
            for(int i = 0; i < gvControls.Count; i++)
            {
                ControlRecord cr = gvControls[i];
                if (cr.Controller == ctr)
                {
                    if (cr.Controller.View.Visible)
                        cr.Controller.ExecuteMessage(MsgViewWillHide);
                    cr.Controller.Parent = null;
                    cr.Controller.View.Parent = null;
                    panel1.Controls.Remove(cr.Controller.View);
                    gvControls.RemoveAt(i);
                    break;
                }
            }

            if (gvControls.Count > 0)
            {
                ControlRecord cr = LastControl;
                cr.Controller.View.Visible = true;
                LayoutControl(cr.Controller, cr.Align, cr.OriginalSize);
            }
        }

        public void RemoveAll()
        {
            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                if (panel1.Controls[i].Visible)
                    gvControls[i].Controller.ExecuteMessage(MsgViewWillHide);
                panel1.Controls[i].Parent = null;
            }
            panel1.Controls.Clear();
        }

        public void HideControlsAll(bool alsoLast)
        {
            int top = panel1.Controls.Count - 1;
            if (alsoLast)
                top++;
            for (int i = 0; i < top; i++)
            {
                if (panel1.Controls[i].Visible)
                    gvControls[i].Controller.ExecuteMessage(MsgViewWillHide);
                panel1.Controls[i].Visible = false;
            }
        }

        public void InitialLayoutControl(GVCore controller, GVControlAlign align, Size OriginalSize)
        {
            Control child = controller.View;
            if (align == GVControlAlign.Center)
            {
                Size cSize = new Size(Math.Min(OriginalSize.Width, panel1.Size.Width),
                    Math.Min(OriginalSize.Height, panel1.Size.Height));
                Size pSize = panel1.Size;
                child.Location = new Point(pSize.Width / 2 - cSize.Width / 2, pSize.Height / 2 - cSize.Height / 2);
                child.Size = cSize;
                child.Anchor = AnchorStyles.None;
                panel1.AutoScroll = false;
            }
            else if (align == GVControlAlign.Fill)
            {
                child.Location = new Point(0, 0);
                child.Size = panel1.Size;
                panel1.AutoScroll = false;
                child.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            }
            else if (align == GVControlAlign.Scroll)
            {
                child.Location = new Point(0, 0);
                child.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                child.AutoScrollOffset = new Point(0, 0);
                panel1.AutoScroll = true;
                panel1.Refresh();
            }
        }

        public void LayoutControl(GVCore controller, GVControlAlign align, Size OriginalSize)
        {
            Control child = controller.View;
            if (align == GVControlAlign.Center)
            {
                Size cSize = new Size(Math.Min(OriginalSize.Width, panel1.Size.Width),
                    Math.Min(OriginalSize.Height, panel1.Size.Height));
                Size pSize = this.Size;
                child.Location = new Point(pSize.Width / 2 - cSize.Width / 2, pSize.Height / 2 - cSize.Height / 2);
                child.Size = cSize;
            }
            else if (align == GVControlAlign.Fill)
            {
            }
            else if (align == GVControlAlign.Scroll)
            {
            }
        }
    }

}
