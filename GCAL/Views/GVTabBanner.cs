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
    /// <summary>
    /// This class is implementation of dynamic Tab control
    /// </summary>
    public partial class GVTabBanner : UserControl, IControlContainer
    {
        public const string MsgTabWillHide = "tabBannerTabWillHide";
        public const string MsgTabChanged = "tabBannerTabChanged";

        /// <summary>
        /// Helper record for storing info about tabs
        /// </summary>
        public class GVTabBannerTab
        {
            public string DisplayName { get; set; }
            public string TagName { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }

            public GVTabBannerTab()
            {
                Width = -1;
                Height = -1;
            }
        }

        private GVCore p_delegate = null;
        public GVCore Controller 
        {
            set
            {
                p_delegate = value;
                containerProxy.Parent = value;
            }
            get
            {
                return p_delegate;
            }
        }

        private List<GVTabBannerTab> Tabs = new List<GVTabBannerTab>();
        private List<GVTabBannerTab> DisplayedTabs = new List<GVTabBannerTab>();
        private List<GVTabBannerTab> HiddenTabs = new List<GVTabBannerTab>();
        private GVTabBannerTab MoreItemsTab = new GVTabBannerTab();
        private string p_selectedTab = null;
        private float BarHeight = -1;
        private Point trackedPoint;
        private ContextMenu contextMoreItems = new ContextMenu();
        public GVControlContainerController containerProxy = null;

        public GVTabBanner()
        {
            InitializeComponent();
            MoreItemsTab.DisplayName = "***";
            MoreItemsTab.TagName = "_more";

            containerProxy = new GVControlContainerController(gvControlContainer1);
        }

        public string SelectedTab
        {
            get
            {
                return p_selectedTab;
            }
            set
            {
                SelectTab(value);
            }
        }

        protected float getWidthSum()
        {
            float sx = 0;
            foreach (GVTabBannerTab tb in Tabs)
            {
                sx += tb.Width;
            }
            return sx;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            float space = 3;
            float padding = 8;
            float currX = space;
            float currY = 0;
            float prevBarHeight = BarHeight;

            Graphics g = e.Graphics;

            CalculateTabWidths(space, padding, g);

            SortTabsVisibility(padding);

            currY = BarHeight;

            for (int i = 0; i < DisplayedTabs.Count; i++)
            {
                GVTabBannerTab tab = DisplayedTabs[i];

                if (tab.TagName.Equals(p_selectedTab))
                {
                    g.FillRectangle(SystemBrushes.MenuHighlight, currX + space, 0, tab.Width - 2 * space, tab.Height);
                    g.DrawString(tab.DisplayName, SystemFonts.MenuFont, SystemBrushes.HighlightText, currX + space + padding, padding);
                }
                else
                {
                    g.DrawString(tab.DisplayName, SystemFonts.MenuFont, SystemBrushes.MenuText, currX + space + padding, padding);
                    g.DrawLine(Pens.Black, currX, currY, currX + tab.Width, currY);
                }

                if ((trackedPoint.X >= currX && trackedPoint.X <= currX + tab.Width)
                    || (tab.TagName.Equals(p_selectedTab)))
                {
                    PointF[] points = new PointF[] {
                        new PointF(currX, currY),
                        new PointF(currX + space, currY),
                        new PointF(currX + space, 0),
                        new PointF(currX + tab.Width - space, 0),
                        new PointF(currX + tab.Width - space, currY),
                        new PointF(currX + tab.Width, currY)
                    };

                    g.DrawLines(Pens.Black, points);
                }

                currX += tab.Width;
            }

            g.DrawLine(Pens.Black, currX, currY, this.Width, currY);

            if (prevBarHeight != BarHeight)
            {
                gvControlContainer1.Location = new Point(0, (int)BarHeight + 1);
                gvControlContainer1.Size = new Size(Width, Height - (int)BarHeight - 1);
            }

            base.OnPaint(e);
        }

        private void CalculateTabWidths(float space, float padding, Graphics g)
        {
            float bh = 0;

            // first ensure that all tabs have width
            foreach (GVTabBannerTab tab in Tabs)
            {
                if (tab.Width < 0)
                {
                    SizeF sf = g.MeasureString(tab.DisplayName, SystemFonts.MenuFont);
                    tab.Width = sf.Width + 2 * padding + 2 * space;
                    tab.Height = sf.Height + 2 * padding;
                }

                bh = Math.Max(bh, tab.Height);
            }

            if (MoreItemsTab.Width < 0)
            {
                SizeF sf = g.MeasureString(MoreItemsTab.DisplayName, SystemFonts.MenuFont);
                MoreItemsTab.Width = sf.Width + 2 * padding + 2 * space;
                MoreItemsTab.Height = sf.Height + 2 * padding;
            }

            BarHeight = bh;
        }

        /// <summary>
        /// Splits Tabs into two groups: Visible, Hidden
        /// If Tabs cannot be displayed in one row, then last tab is showed as "More Items"
        /// and selected tab is displayed as first.
        /// If Tabs can be displayed, then all tabs are drawn in the natural order
        /// </summary>
        /// <param name="padding"></param>
        /// <param name="currX"></param>
        private void SortTabsVisibility(float padding)
        {
            DisplayedTabs.Clear();
            HiddenTabs.Clear();

            // check the width of all tabs
            if (getWidthSum() > this.Width)
            {
                float cx = 0;
                // first add selected tab
                foreach (GVTabBannerTab tab in Tabs)
                {
                    if (tab.TagName.Equals(p_selectedTab))
                    {
                        DisplayedTabs.Add(tab);
                        cx += tab.Width;
                        break;
                    }
                }

                // then add as much tabs as possible
                bool hiddenTabs = false;
                foreach (GVTabBannerTab tab in Tabs)
                {
                    if (tab.TagName.Equals(p_selectedTab))
                        continue;

                    if (!hiddenTabs)
                    {
                        if (cx + tab.Width + MoreItemsTab.Width > this.Width)
                        {
                            DisplayedTabs.Add(MoreItemsTab);
                            cx += MoreItemsTab.Width;
                            hiddenTabs = true;
                        }
                        else
                        {
                            DisplayedTabs.Add(tab);
                            cx += tab.Width;
                        }
                    }

                    if (hiddenTabs)
                    {
                        HiddenTabs.Add(tab);
                    }
                }
            }
            else
            {
                DisplayedTabs.AddRange(Tabs);
            }
        }

        private string getTagFromIndex(int index)
        {
            if (index < 0 || index >= Tabs.Count)
                return null;
            return Tabs[index].TagName;
        }

        private bool ExistsTag(string nt)
        {
            foreach (GVTabBannerTab tab in Tabs)
            {
                if (tab.TagName.Equals(nt))
                    return true;
            }
            return false;
        }

        private int getTabIndexFromPoint(Point p)
        {
            float cx = 0;
            int i = 0;
            foreach (GVTabBannerTab tab in DisplayedTabs)
            {
                if (p.X >= cx && p.X <= cx + tab.Width && p.Y >= 0 && p.Y <= tab.Height)
                {
                    return i;
                }
                i++;
                cx += tab.Width;
            }

            return -1;
        }

        private void GVTabBanner_MouseMove(object sender, MouseEventArgs e)
        {
            trackedPoint = e.Location;
            Invalidate();
        }

        private int p_md_indexTab = 0;

        private void GVTabBanner_MouseDown(object sender, MouseEventArgs e)
        {
            p_md_indexTab = getTabIndexFromPoint(e.Location);

        }

        private void GVTabBanner_MouseUp(object sender, MouseEventArgs e)
        {
            if (getTabIndexFromPoint(e.Location) == p_md_indexTab && p_md_indexTab >= 0)
            {
                int i = p_md_indexTab;
                p_md_indexTab = -1;
                GVTabBannerTab selected = DisplayedTabs[i];
                if (selected.TagName.Equals("_more"))
                {
                    contextMoreItems.MenuItems.Clear();
                    foreach (GVTabBannerTab tab in HiddenTabs)
                    {
                        MenuItem mi = contextMoreItems.MenuItems.Add(tab.DisplayName);
                        mi.Tag = tab.TagName;
                        mi.Click += menuItemToolStripMenuItem_Click;
                    }
                    contextMoreItems.Show(this, new Point(e.X, e.Y));
                }
                else
                {
                    SelectTab(selected.TagName);
                }
            }
            else
            {
                p_md_indexTab = -1;
            }
        }

        private void menuItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is MenuItem)
            {
                SelectTab((sender as MenuItem).Tag as string);
            }
        }

        private void GVTabBanner_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Adds new tab to the list
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public GVTabBannerTab AddTab(string displayName, string tagName)
        {
            GVTabBannerTab newtab = new GVTabBannerTab();
            newtab.DisplayName = displayName;
            newtab.TagName = tagName;

            Tabs.Add(newtab);

            Invalidate();

            return newtab;

        }

        /// <summary>
        /// Selects Tab as active
        /// </summary>
        /// <param name="tabTag"></param>
        /// <returns></returns>
        public bool SelectTab(string tabTag)
        {
            if (Controller != null && p_selectedTab != null)
            {
                Controller.ExecuteMessage(MsgTabWillHide, new GSString(p_selectedTab));
                GVTabBannerTab tabBanner = FindTab(tabTag);
            }
            p_selectedTab = tabTag;
            if (ExistsTag(tabTag))
            {
                if (Controller != null)
                    Controller.ExecuteMessage(MsgTabChanged, new GSString(tabTag));
                Invalidate();
                return true;
            }

            return false;
        }

        public GVTabBannerTab FindTab(string tag)
        {
            foreach (GVTabBannerTab t in Tabs)
            {
                if (t.TagName.Equals(tag))
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Displays user control in the area reserved for user content
        /// </summary>
        /// <param name="userControl"></param>
        public void AddControl(GVCore userController, GVControlAlign align)
        {
            gvControlContainer1.AddControl(userController, align);
        }

        public void RemoveControl(GVCore ctrl)
        {
            gvControlContainer1.RemoveControl(ctrl);
        }

        public void RemoveAll()
        {
            gvControlContainer1.RemoveAll();
        }

        private void GVTabBanner_MouseLeave(object sender, EventArgs e)
        {
            // set tracked point to somewhere out of client area
            trackedPoint = new Point(-10, 0);
            Invalidate();
        }
    }

}
