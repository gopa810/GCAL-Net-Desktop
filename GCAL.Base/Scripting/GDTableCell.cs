using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GDTableCell: GDDocumentBlock
    {
        private int p_cellSpan = 1;

        public int getCellSpan()
        {
            return p_cellSpan;
        }

    }
}
