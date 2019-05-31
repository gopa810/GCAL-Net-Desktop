using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base.Scripting;

namespace GCAL.Views
{
    public class GVCore: GSCore
    {
        public Control View { get; set; }

        public IControlContainer ViewContainer { get; set; }

        /// <summary>
        /// This is hierarchy of controllers
        /// </summary>
        public GSCore Parent { get; set; }

        /// <summary>
        /// This is for dialog panels, they are delivering 
        /// message to any controller (not necessarily to parent)
        /// </summary>
        public GSCore Target { get; set; }


        public GVCore(): base()
        {
            Parent = null;
            Target = null;
            View = null;
        }

        public bool HasParent
        {
            get
            {
                return Parent != null;
            }
        }

        public bool HasTarget
        {
            get
            {
                return Target != null;
            }
        }

        public void ShowInContainer(IControlContainer viewContainer, GVControlAlign align)
        {
            ViewContainer = viewContainer;
            if (viewContainer != null)
                viewContainer.AddControl(this, align);
        }

        public void RemoveFromContainer()
        {
            if (ViewContainer != null)
                ViewContainer.RemoveControl(this);
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            if (token.Equals("getParent"))
            {
                return Parent;
            }
            else if (token.Equals("setParent"))
            {
                Parent = args.getSafe(0);
                return Parent;
            }
            else
            {
                return base.ExecuteMessage(token, args);
            }
        }

    }

}
