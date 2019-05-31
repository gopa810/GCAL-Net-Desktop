using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base.Scripting
{
    public class GDStyleDefinition: GDNode
    {
        public string Name { get; set; }

        public GDStyleDefinition()
        {
        }

        public GDStyleDefinition(string name, params object[] args)
        {
            Name = name;
            if (args != null)
            {
                foreach (object obj in args)
                {
                    if (obj is GDTextStyle)
                    {
                        Format.AddStyle(obj as GDTextStyle);
                    }
                }
            }
        }

        public override void ExportRichText(GDExportContext context)
        {
            base.ExportRichText(context);
        }

        public override void ExportHtml(TextWriter stream)
        {
            base.ExportHtml(stream);
        }
    }
}
