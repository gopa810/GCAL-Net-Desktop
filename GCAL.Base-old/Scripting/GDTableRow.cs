using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GDTableRow: GDNode
    {
        public List<GDTableCell> cells = new List<GDTableCell>();

        public GDTableCell NewCell()
        {
            GDTableCell cell = new GDTableCell();
            cells.Add(cell);
            return cell;
        }

        public int getColumnCount()
        {
            int sum = 0;
            foreach (GDTableCell cell in cells)
            {
                sum += cell.getCellSpan();
            }
            return sum;
        }

    }
}
