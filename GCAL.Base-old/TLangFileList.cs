using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class TLangFileList
    {
        public List<TLangFileInfo> list = new List<TLangFileInfo>();
        public TLangFileInfo add()
        {
            TLangFileInfo p = new TLangFileInfo();
            list.Add(p);
            return p;
        }
        void clear()
        {
            list.Clear();
        }
    }
}
