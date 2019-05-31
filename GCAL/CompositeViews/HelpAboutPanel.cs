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
    public partial class HelpAboutPanel : UserControl
    {
        public HelpAboutPanelController Controller { get; set; }

        public HelpAboutPanel()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }
    }

    public class HelpAboutPanelController : GVCore
    {
        public HelpAboutPanelController(HelpAboutPanel v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
