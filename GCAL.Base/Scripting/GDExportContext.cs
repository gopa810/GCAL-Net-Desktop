using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base.Scripting
{
    public class GDExportContext
    {
        public TextWriter Stream;

        public Dictionary<object, int> fontMaps;

        public Dictionary<object, int> colorMaps;
    }
}
