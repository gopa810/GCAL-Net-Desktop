using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;
using GCAL.Base.Properties;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public partial class RatedEventsEditor : UserControl
    {
        public RatedEventsEditorController Controller { get; set; }

        public event TBButtonPressed OnConfListChanged;

        public Font BoldFont = new Font(SystemFonts.CaptionFont, FontStyle.Bold);
        public Font TextFont = new Font(SystemFonts.CaptionFont, FontStyle.Regular);
        public int Padding = 8;
        public int Spacing = 4;

        public RatedEventsEditor()
        {
            InitializeComponent();

            InitListConfigs();

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        private void InitListConfigs()
        {
            listBox1.Items.Clear();
            foreach (GCConfigRatedEvents ge in GCConfigRatedManager.Configurations)
            {
                ListBoxRichItem lb = new ListBoxRichItem();
                lb.Title = ge.Title;
                lb.Subtitle = ge.Description;
                lb.Tag = ge;
                listBox1.Items.Add(lb);
            }
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                e.ItemHeight = (listBox1.Items[e.Index] as ListBoxRichItem).MeasureItem(e.Graphics, this.Bounds, 1);
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                (listBox1.Items[e.Index] as ListBoxRichItem).DrawItem(e, 1);
            }
        }

        private void toolStripButtonExit_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

        /// <summary>
        /// Creating new configuration
        /// - get title
        /// - save to disk
        /// - add to configurations
        /// - add to combobox in main RatedEventsTab
        /// - show new configuration in RatedEventsDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            RatedEventNewTitle d = new RatedEventNewTitle();
            d.OnButtonYes += new TBButtonPressed(OnNewConfiguration);
            d.TextLabel = "Create new configuration for rated events.";
            d.TextValue = "Untitled";
            RatedEventNewTitleController dc = new RatedEventNewTitleController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        /// <summary>
        /// Callback from new title dialog
        /// this is received in two cases:
        /// - creation of the new configuration
        /// - cloning of the existing configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewConfiguration(object sender, EventArgs e)
        {
            if (sender is RatedEventNewTitle)
            {
                RatedEventNewTitle d = sender as RatedEventNewTitle;
                GCConfigRatedEvents ge = GCConfigRatedManager.CreateConfiguration(d.TextValue);
                if (d.ExistingConfiguration != null)
                    ge.CopyFrom(d.ExistingConfiguration);
                if (OnConfListChanged != null) OnConfListChanged(this, e);

                RatedEventDetails d1 = new RatedEventDetails();
                if (OnConfListChanged != null)
                    d1.OnConfigurationChanged += new TBButtonPressed(OnConfListChanged);
                d1.setConfig(ge);
                RatedEventDetailsController d1c = new RatedEventDetailsController(d1);
                d1c.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            if (SelectedTemplateConfig != null)
            {
                RatedEventDetails d1 = new RatedEventDetails();
                if (OnConfListChanged != null)
                    d1.OnConfigurationChanged += new TBButtonPressed(OnConfListChanged);
                d1.setConfig(SelectedTemplateConfig);
                RatedEventDetailsController d1c = new RatedEventDetailsController(d1);
                d1c.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            RatedEventNewTitle d = new RatedEventNewTitle();
            d.OnButtonYes += new TBButtonPressed(OnNewConfiguration);
            d.ExistingConfiguration = SelectedTemplateConfig;
            d.TextLabel = "Clone existing configuration for rated events.";
            d.TextValue = (d.ExistingConfiguration != null ? d.ExistingConfiguration.Title : "Untitled") + " (copy)";
            d.ButtonOkLabel = "Clone";
            RatedEventNewTitleController dc = new RatedEventNewTitleController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (GCConfigRatedManager.Configurations.Count < 2)
                return;

            GCConfigRatedEvents ev = SelectedTemplateConfig;
            if (ev != null)
            {
                // ask for confirmation
                AskDeleteObject d = new AskDeleteObject();
                d.InitialLabel = "Are you sure to delete following configuration?";
                d.DetailLabel = ev.Title;
                d.OnButtonYes +=new TBButtonPressed(onDeleteConfigTemplateYes);
                d.Tag = ev;
                AskDeleteObjectController dc = new AskDeleteObjectController(d);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        private void onDeleteConfigTemplateYes(object sender, EventArgs e)
        {
            if (sender is AskDeleteObject)
            {
                AskDeleteObject d = sender as AskDeleteObject;
                GCConfigRatedManager.DeleteConfiguration(d.Tag as GCConfigRatedEvents);
                InitListConfigs();
            }
        }

        private GCConfigRatedEvents SelectedTemplateConfig
        {
            get
            {
                if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
                {
                    ListBoxRichItem lbi = listBox1.Items[listBox1.SelectedIndex] as ListBoxRichItem;
                    return lbi.Tag as GCConfigRatedEvents;
                }
                return null;
            }
        }
    }

    public class RatedEventsEditorController : GVCore
    {
        public RatedEventsEditorController(RatedEventsEditor re)
        {
            View = re;
            re.Controller = this;
        }
    }
}
