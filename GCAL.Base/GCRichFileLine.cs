using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCRichFileLine
    {
        public string Tag { get; set; }

	    private string[] p_fields = null;

        public int TagInt
        {
            get
            {
                int i = 0;
                if (int.TryParse(Tag, out i))
                    return i;
                return 0;
            }
            set
            {
                Tag = value.ToString();
            }
        }

        public bool SetLine(string line)
        {
            if (line == null)
                return false;

            int rc = 0;

	        // analyza riadku
            rc = line.IndexOf(' ');
            if (rc > 0)
            {
                Tag = line.Substring(0, rc);
                p_fields = line.Substring(rc + 1).Split('|');
                for (int j = 0; j < p_fields.Length; j++)
                    p_fields[j] = GCFestivalBase.SafeToString(p_fields[j]);
            }
            else
            {
                Tag = string.Empty;
                p_fields = new string[]{};
            }

	        return true;
        }

        public string GetField(int i)
        {
            return this[i];
        }

        public int GetFieldInt(int i)
        {
            return int.Parse(this[i]);
        }

        public double GetFieldDouble(int i)
        {
            return double.Parse(this[i]);
        }

        public string this[int i]
        {
            get
            {
                if (p_fields == null)
                    return string.Empty;
                if (i < 0 || i >= p_fields.Length)
                    return string.Empty;
                return p_fields[i];
            }
        }

    }
}
