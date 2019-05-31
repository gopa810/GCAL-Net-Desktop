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
    public partial class GVListBanner : UserControl
    {
        public const string MsgListItemChanged = "listBannerTabChanged";

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

            public override string ToString()
            {
                return DisplayName;
            }
        }

        public GVCore Controller { get; set; }
        private string p_selectedTab = null;
        private int padding = 8;
        private int space = 3;
        private Font captionFont = null;

        public GVListBanner()
        {
            InitializeComponent();
            Controller = null;
            captionFont = new Font(SystemFonts.MenuFont, FontStyle.Bold);
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            GVTabBannerTab tab = listBox1.Items[e.Index] as GVTabBannerTab;
            if (tab == null)
                return;

            if (tab.Height < 0)
            {
                SizeF sf = e.Graphics.MeasureString(tab.DisplayName, SystemFonts.MenuFont);
                tab.Width = sf.Width + 2 * padding + 2 * space;
                tab.Height = sf.Height + 2 * space;
            }

            if (tab.TagName == null)
                tab.Height += padding;

            e.ItemHeight = (int)tab.Height;
        }

        private int previousSelectedIndex = -1;
        private bool bInhbit = false;

        public int SelectedIndex
        {
            get
            {
                return listBox1.SelectedIndex;
            }
            set
            {
                listBox1.SelectedIndex = value;
            }
        }

        public int SelectedIndexNoResponse
        {
            get
            {
                return listBox1.SelectedIndex;
            }
            set
            {
                bInhbit = true;
                listBox1.SelectedIndex = value;
                bInhbit = false;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bInhbit)
                return;

            int eIndex = listBox1.SelectedIndex;

            if (eIndex >= 0 && eIndex < listBox1.Items.Count)
            {
                GVTabBannerTab tab = listBox1.Items[eIndex] as GVTabBannerTab;

                if (tab.TagName == null)
                {
                    SelectedIndexNoResponse = previousSelectedIndex;
                }
                else
                {
                    previousSelectedIndex = eIndex;
                    if (Controller != null)
                    {
                        Controller.ExecuteMessage(MsgListItemChanged, new GSString(tab.TagName));
                    }
                }

            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                GVTabBannerTab tab = listBox1.Items[e.Index] as GVTabBannerTab;

                if ((e.State & DrawItemState.Selected) != 0 && tab.TagName != null)
                {
                    e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, e.Bounds);
                }
                else if ((e.State & DrawItemState.HotLight) != 0)
                {
                    e.Graphics.FillRectangle(SystemBrushes.HotTrack, e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Menu, e.Bounds);
                }


                if (tab.TagName == null)
                {
                    e.Graphics.DrawString(tab.DisplayName, captionFont, SystemBrushes.MenuText, padding, e.Bounds.Top + padding + space);
                }
                else
                {
                    e.Graphics.DrawString(tab.DisplayName, SystemFonts.MenuFont, SystemBrushes.MenuText, 2*padding, e.Bounds.Top + space);
                }
            }
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

            listBox1.Items.Add(newtab);
            return newtab;

        }

        private bool ExistsTag(string nt)
        {
            foreach (GVTabBannerTab tab in listBox1.Items)
            {
                if (tab.TagName.Equals(nt))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Selects Tab as active
        /// </summary>
        /// <param name="tabTag"></param>
        /// <returns></returns>
        public bool SelectTab(string tabTag)
        {
            p_selectedTab = tabTag;
            if (ExistsTag(tabTag))
            {
                if (Controller != null)
                    Controller.ExecuteMessage(MsgListItemChanged, new GSString(tabTag));
                Invalidate();
                return true;
            }

            return false;
        }
    }
}
