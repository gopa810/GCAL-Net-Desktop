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
using GCAL.CompositeViews;

namespace GCAL.CompositeViews
{
    public partial class HelpPanel : UserControl
    {
        public HelpPanelController Controller { get; set; }

        public HelpPanel()
        {
            InitializeComponent();
        }

        public RichTextBox RichTextBox
        {
            get
            {
                return richTextBox1;
            }
        }
    }

    public class HelpPanelController : GVCore
    {
        public HelpPanelController(HelpPanel v)
        {
            View = v;
            v.Controller = this;
        }

        public HelpPanel getView()
        {
            return View as HelpPanel;
        }

        public void ShowRichText(string p)
        {
            getView().RichTextBox.Rtf = p;
        }

        public void ShowPlainText(string p)
        {
            getView().RichTextBox.Text = p;
        }
    }
}
