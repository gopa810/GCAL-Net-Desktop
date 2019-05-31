using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;
using GCAL.Views;

namespace GCAL.Views
{
    public class GVTabBannerController : GVCore
    {
        public GVTabBannerController(GVTabBanner v)
        {
            View = v;
            v.Controller = this;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            if (Parent != null)
            {
                return Parent.ExecuteMessage(token, args);
            }
            else
            {
                return base.ExecuteMessage(token, args);
            }
        }

    }

}
