using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base.Scripting
{
    public class GDNode
    {
        private GDNodeCollection p_nodes = null;

        private GDTextStyleCollection p_format = null;

        public GDNode ParentNode { get; set; }

        public GDNodeCollection Nodes
        {
            get
            {
                if (p_nodes == null)
                {
                    GDNodeCollection nc = new GDNodeCollection();
                    nc.Parent = this;
                    return nc;
                }
                return p_nodes;
            }

            set
            {
                p_nodes = value;
            }
        }

        public GDTextStyleCollection Format
        {
            get
            {
                if (p_format == null)
                {
                    GDTextStyleCollection nf = new GDTextStyleCollection();
                    nf.Parent = this;
                    return nf;
                }
                return p_format;
            }
            set
            {
                p_format = value;
            }
        }

        public Dictionary<object, int> HarvestFormatValues(params GDStyleKey[] key)
        {
            Dictionary<object, int> data = new Dictionary<object, int>();
            int topID = 1;

            foreach (GDStyleKey k1 in key)
            {
                _HarvestFormatValues(data, ref topID, k1);
            }
            return data;
        }

        private void _HarvestFormatValues(Dictionary<object, int> data, ref int topID, GDStyleKey key)
        {
            if (Format.ContainsStyle(key))
            {
                object obj = Format.GetValue(key);
                if (obj != null)
                {
                    if (!data.ContainsKey(obj))
                    {
                        data.Add(obj, topID);
                        topID++;
                    }
                }
            }

            if (p_nodes != null)
            {
                foreach (GDNode node in p_nodes)
                {
                    node._HarvestFormatValues(data, ref topID, key);
                }
            }
        }

        public virtual void ExportPlainText(TextWriter stream)
        {
        }

        public virtual void ExportRichText(GDExportContext context)
        {
        }

        public virtual void ExportHtml(TextWriter stream)
        {
        }

    }
}
