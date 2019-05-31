using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GDTable: GDNode
    {
        public List<GDTableRow> rows = new List<GDTableRow>();

        public GDTableRow NewRow()
        {
            GDTableRow row = new GDTableRow();
            rows.Add(row);
            return row;
        }

        /// <summary>
        /// Returns count of columns in the table. Value is retrieved as maximum
        /// of columns for all rows.
        /// </summary>
        /// <returns></returns>
        public int getColumnCount()
        {
            int max = 0;
            int cols = 0;
            foreach(GDTableRow row in rows)
            {
                cols = row.getColumnCount();
                max = Math.Max(max, cols);
            }
            return max;
        }
    }
}
